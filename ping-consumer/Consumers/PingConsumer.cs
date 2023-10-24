using contracts;
using MassTransit;

namespace Consumers;

public class PingConsumer : IConsumer<PingMessage>
{
    private readonly ILogger<PingConsumer> _logger;

    public PingConsumer(ILogger<PingConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<PingMessage> context)
    {
        _logger.LogInformation($"Received ping: {context.Message.Text}");

        return Task.CompletedTask;
    }
}