using Application.Common.Interfaces;
using Application.IntegrationEvents;
using Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Products.EventHandlers
{
    public class ProductUpdatedEventHandler : INotificationHandler<ProductUpdatedEvent>
    {
        private readonly ILogger<ProductUpdatedEventHandler> _logger;
        private readonly IIntegrationEventService _integrationEventService;

        public ProductUpdatedEventHandler(
            ILogger<ProductUpdatedEventHandler> logger,
            IIntegrationEventService integrationEventService)
        {
            _logger = logger;
            _integrationEventService = integrationEventService;
        }

        public async Task Handle(ProductUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Product updated: {ProductId} - {ProductName}",
                notification.Product.Id,
                notification.Product.Name.Value);

            // Publish integration event for external systems
            var integrationEvent = new ProductStockUpdatedIntegrationEvent
            {
                ProductId = notification.Product.Id,
                ProductName = notification.Product.Name.Value,
                NewQuantity = notification.Product.StockQuantity,
                UpdatedAt = DateTime.UtcNow
            };

            await _integrationEventService.PublishAsync(integrationEvent, cancellationToken);
        }
    }
}
