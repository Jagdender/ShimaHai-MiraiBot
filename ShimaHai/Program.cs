using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mirai.CSharp.Builders;
using Mirai.CSharp.HttpApi.Builder;
using Mirai.CSharp.HttpApi.Invoking;
using Mirai.CSharp.HttpApi.Session;
using ShimaHai.Handlers;
using ShimaHai.Parsers;

var builder = Host.CreateApplicationBuilder();

builder.Services.AddDefaultMiraiHttpFramework().AddParser<UnnamedParser>().AddInvoker<MiraiHttpMessageHandlerInvoker>().AddHandler<DisconnectHandler>().AddClient<MiraiHttpSession>().Services.AddLogging().BuildServiceProvider() ;

IHost host = builder.Build();

await host.StartAsync();

await host.WaitForShutdownAsync();