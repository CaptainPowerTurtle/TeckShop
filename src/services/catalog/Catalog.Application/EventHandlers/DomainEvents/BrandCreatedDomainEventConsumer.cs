using Catalog.Domain.Entities.Brands;
using MassTransit;
using Microsoft.Extensions.Logging;
using Teckshop.Events;

namespace Catalog.Application.EventHandlers.DomainEvents
{
    /// <summary>
    /// The brand created domain event consumer.
    /// </summary>
    public class BrandCreatedDomainEventConsumer : IConsumer<BrandCreatedDomainEvent>
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<BrandCreatedDomainEventConsumer> _logger;

        /// <summary>
        /// Publish endpoint.
        /// </summary>
        private readonly IPublishEndpoint _publishEndpoint;

        /// <summary>
        /// Initializes a new instance of the <see cref="BrandCreatedDomainEventConsumer"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="publishEndpoint"></param>
        public BrandCreatedDomainEventConsumer(ILogger<BrandCreatedDomainEventConsumer> logger, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }

        /// <summary>
        /// Consume the domain event.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "AV1755:Name of async method should end with Async or TaskAsync", Justification = "Masstransit consumer name should not contain async: https://masstransit.io/documentation/concepts/consumers")]
        public async Task Consume(ConsumeContext<BrandCreatedDomainEvent> context)
        {
            _logger.LogInformation($"Message is {{message}}", context.Message);

            var @event = new BrandCreatedIntegrationEvent(context.Message.BrandId);
            await _publishEndpoint.Publish(@event, cancellationToken: context.CancellationToken);
        }
    }
}
