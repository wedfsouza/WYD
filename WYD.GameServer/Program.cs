using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WYD.GameServer;
using WYD.Infrastructure;

var builder = Host.CreateApplicationBuilder();

builder.Services.AddSingleton<TaskChannel>();
builder.Services.AddSingleton<ITaskProducer, TaskProducer>();
builder.Services.AddHostedService<TaskConsumer>();

builder.Services.AddSingleton<IGameServerSessionHandler, GameServerSessionHandler>();

builder.Services.AddSingleton(serviceProvider =>
{
    var gameServerSessionHandler = serviceProvider.GetRequiredService<IGameServerSessionHandler>();
    var taskProducer = serviceProvider.GetRequiredService<ITaskProducer>();
    var protocol = new GameServerProtocol(gameServerSessionHandler, taskProducer);
    var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

    return new GameServerListener("127.0.0.1", 8281, protocol, loggerFactory);
});

var host = builder.Build();

var gameServerListener = host.Services.GetRequiredService<GameServerListener>();
gameServerListener.Start();

await host.RunAsync();