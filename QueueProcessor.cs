using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FunctionHostAppRepo;

/// <summary>
/// Queue-triggered function used with Lab 1's broken host.json.
///
/// The broken host.json in lab1-files/ sets:
///   - extensions.queues.batchSize = 16   (will apply once host.json is fixed)
///   - extensions.queues.queueName       (invalid - belongs in function.json)
///
/// This function listens to the queue named "my-queue" (the one the broken
/// host.json incorrectly tries to set globally) so the lab exercise wires up
/// end-to-end once the student fixes host.json.
/// </summary>
public class QueueProcessor
{
    private readonly ILogger<QueueProcessor> _logger;

    public QueueProcessor(ILogger<QueueProcessor> logger)
    {
        _logger = logger;
    }

    [Function(nameof(QueueProcessor))]
    public void Run(
        [QueueTrigger("My_Queue!", Connection = "MyStorageConnection")] string message)
    {
        _logger.LogInformation("Queue message received: {Message}", message);
    }
}
