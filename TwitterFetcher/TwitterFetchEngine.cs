using Config;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;

using Timer = System.Timers.Timer;

namespace TwitterFetcher
{
    public class TwitterFetchEngine : IAsyncDisposable
    {
        private readonly AppSettings _options;
        private readonly ILogger<TwitterFetchEngine> _logger;

        private IBrowserContext? _context;
        private IPlaywright? _engine;
        private readonly ICollection<string> _subscribers = new List<string>();
        private readonly Timer _timer = new();

        public TwitterFetchEngine(IOptions<AppSettings> options, ILogger<TwitterFetchEngine> logger)
        {
            _options = options.Value;
            _logger = logger;
        }

        public Action<IEnumerable<string>> OutputFetchedTweets { get; set; } = null!;

        public void Start(TimeSpan time)
        {
            _timer.Interval = time.TotalMilliseconds;
            _timer.Elapsed += async (_, _) =>
            {
                OutputFetchedTweets?.Invoke(await FetchAsync());
            };
            _timer.Start();
        }

        public async Task InitializeAsync()
        {
            _logger.LogInformation("Initializing twitter fetch engine...");
            _engine = await Playwright.CreateAsync();
            var browser = await _engine.Chromium.LaunchAsync(
                new() { Channel = "msedge", Headless = true }
            );
            _context = await browser.NewContextAsync();
            await _context.AddCookiesAsync(
                new List<Cookie>
                {
                    new()
                    {
                        Url = _options.Twitter.Url,
                        Name = "auth_token",
                        Value = _options.Twitter.AuthToken
                    },
                    new()
                    {
                        Url = _options.Twitter.Url,
                        Name = "twid",
                        Value = _options.Twitter.Twid
                    }
                }
            );
            _logger.LogInformation("Initialize successfully.");
        }

        public void Subscribe(string username)
        {
            _subscribers.Add(username);
        }

        public void Subscribe(IEnumerable<string> subscribers)
        {
            foreach (var subscriber in subscribers)
            {
                _subscribers.Add(subscriber);
            }
        }

        public async Task<IEnumerable<string>> FetchAsync()
        {
            _logger.LogInformation("Start fetching tweets...");
            if (_engine is null || _context is null)
            {
                await InitializeAsync();
            }

            ICollection<string> uris = new List<string>();
            IPage page = await _context!.NewPageAsync();
            foreach (var subscriber in _subscribers)
            {
                try
                {
                    _ = page.GotoAsync(
                        _options.Twitter.Url + "/" + subscriber + "/media",
                        new() { Timeout = 200000 }
                    );
                    var imagesUris = await PullImagesAsync(page);
                    foreach (var imageUri in imagesUris)
                        uris.Add(imageUri);
                }
                catch (Exception e)
                {
                    _logger.LogError($"Failed to fetch tweets from {subscriber}. For {e.Message}");
                }
            }
            _ = page.CloseAsync();
            _logger.LogInformation("Successfully fetched " + uris.Count + " tweets.");
            return uris;
        }

        public async Task FetchAsync(Action<IEnumerable<string>> output)
        {
            var uris = await FetchAsync();
            output(uris);
        }

        private async Task<IEnumerable<string>> PullImagesAsync(IPage page)
        {
            await page.WaitForSelectorAsync(
                "article",
                new() { State = WaitForSelectorState.Attached }
            );
            var tweets = await page.Locator("article").AllAsync();
            ICollection<string> images = new HashSet<string>();
            foreach (var tweet in tweets)
            {
                var id = await GetIdAsync(tweet);
                var path = Path.Combine(_options.Uri.TwiImage, id + ".jpg");
                if (id is null || File.Exists(path))
                    break;
                else
                {
                    _logger.LogInformation("Successfully pulled tweet " + id);
                    await page.WaitForLoadStateAsync(
                        LoadState.NetworkIdle,
                        new() { Timeout = 200000 }
                    );
                    await tweet.ScreenshotAsync(new() { Path = Path.Combine(path) });
                    images.Add(path);
                }
            }
            return images;
        }

        private async Task<string> GetIdAsync(ILocator locator)
        {
            string href = (
                await (await locator.Locator("a").AllAsync())
                    .Skip(3)
                    .First()
                    .GetAttributeAsync("href", new() { Timeout = 200000 })
            )!;
            string id = href.Split("/").Last();
            return id;
        }

        public async ValueTask DisposeAsync()
        {
            if (_context is not null)
                await _context.DisposeAsync();
            _engine?.Dispose();
            _context = null;
            _engine = null;
            _subscribers.Clear();
        }
    }
}
