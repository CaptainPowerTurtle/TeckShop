using MassTransit;
using Microsoft.Extensions.Logging;
using Teckshop.Events;

namespace Catalog.Application.EventHandlers.IntegrationEvents
{
    /// <summary>
    /// The brand created domain event consumer.
    /// </summary>
    public class BrandCreatedIntegrationEventConsumer : IConsumer<BrandCreatedIntegrationEvent>
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<BrandCreatedIntegrationEventConsumer> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BrandCreatedIntegrationEventConsumer"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public BrandCreatedIntegrationEventConsumer(ILogger<BrandCreatedIntegrationEventConsumer> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Consume the integration event.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task Consume(ConsumeContext<BrandCreatedIntegrationEvent> context)
        {
            _logger.LogInformation($"Message is {{message}}", context.Message);
            return Task.CompletedTask;
        }
    }
}
