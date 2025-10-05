using Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Products.EventHandlers
{
    public class ProductCreatedEventHandler : INotificationHandler<ProductCreatedEvent>
    {
        private readonly ILogger<ProductCreatedEventHandler> _logger;

        public ProductCreatedEventHandler(ILogger<ProductCreatedEventHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Product created: {ProductId} - {ProductName}",
                notification.Product.Id,
                notification.Product.Name.Value);

            // Có thể thêm logic khác như:
            // - Gửi email thông báo
            // - Cập nhật cache
            // - Gửi sự kiện tới external services
            // - Tạo audit log

            await Task.CompletedTask;
        }
    }
}
