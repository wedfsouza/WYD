using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WYD.Infrastructure;

public class TaskConsumer(ILoggerFactory loggerFactory, TaskChannel taskChannel) : BackgroundService
{
    private readonly ILogger<TaskConsumer> _logger = loggerFactory.CreateLogger<TaskConsumer>();

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var task in taskChannel.Reader.ReadAllAsync(stoppingToken))
        {
            try
            {
                await task();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while executing the task");
            }
        }
    }
}