using Config;
using MessageResolverLib;
using MessageResolverLib.Abstractions;
using MessageResolverLib.Dispatchers;
using MessageResolverLib.Handlers;
using MessageResolverLib.Recipients;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MiraiClient;
using ShimaHai;
using ShimahaiDatabase;
using ShimahaiDatabase.Controllers;

var builder = Host.CreateApplicationBuilder();

var config = builder.Configuration.AddJsonFile("appsettings.json", optional: false).Build();
builder.Services.Configure<AppSettings>(config);

builder.Services
    .AddHostedService<HostService>()
    .AddDbContext<DatabaseContext>(options =>
    {
        options.UseNpgsql(
            builder.Configuration.GetValue<string>("DbConnectionString"),
            options => options.MigrationsAssembly("ShimaHai")
        );
        options.UseSnakeCaseNamingConvention();
        options.UseLoggerFactory(
            LoggerFactory.Create(
                config =>
                    config.AddFilter(
                        (category, level) =>
                            category == DbLoggerCategory.Database.Command.Name
                            && level == LogLevel.Information
                    )
            )
        );
    })
    // TODO: Add Services
    .AddSingleton<ShimahaiClient>()
    .AddSingleton<IFriendController, FriendController>()
    // Add Message Components
    .AddRecipient<ShimahaiRecipient>()
    .AddDispatcher<ShimahaiRecipient, KemonoDispatcher>()
    .AddHandler<FriendQueryHandler>()
    .AddSender<ShimahaiSender>();

IHost host = builder.Build();

await host.RunAsync();
