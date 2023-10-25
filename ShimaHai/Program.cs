using Config;
using MessageResolverLib.Abstractions;
using MessageResolverLib.Dispatchers;
using MessageResolverLib.Handlers;
using MessageResolverLib.Recipients;
using MessageResolverLib.Senders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MiraiClient;
using Serilog;
using ShimaHai;
using ShimahaiDatabase;
using ShimahaiDatabase.Controllers;
using TwitterFetcher;

var builder = Host.CreateApplicationBuilder();

var config = builder.Configuration.AddJsonFile("appsettings.json", optional: false).Build();

builder.Logging
    .ClearProviders()
    .AddSerilog(
        new LoggerConfiguration().MinimumLevel.Information().WriteTo.Console().CreateLogger()
    );

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
    .AddSingleton<FriendController>()
    .AddSingleton<TwitterFetchEngine>()
    // Add Message Components
    .AddDispatcher<ShimahaiRecipient, KemonoDispatcher>()
    .AddRecipient<ShimahaiRecipient>()
    .AddHandler<FriendQueryHandler>()
    .AddSender<ShimahaiSender>();

IHost host = builder.Build();

await host.RunAsync();
