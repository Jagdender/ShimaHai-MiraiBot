using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShimaHai.Handlers;

var builder = Host.CreateApplicationBuilder();

builder.Services.AddSingleton();

IHost host = builder.Build();

await host.RunAsync();


