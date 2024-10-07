using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WYD.GameServer;

var builder = Host.CreateApplicationBuilder();

builder.Services.AddSingleton(serviceProvider =>
{
    var protocol = new GameServerProtocol();
    var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

    return new GameServerListener("127.0.0.1", 8281, protocol, loggerFactory);
});

var host = builder.Build();

var gameServerListener = host.Services.GetRequiredService<GameServerListener>();
gameServerListener.Start();

await host.RunAsync();