using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Domain.Repositories;
using Domain.ValueObjects;
using MediatR;

namespace Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<Guid>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IApplicationDbContext _context;
        private readonly IDomainEventService _domainEventService;

        public CreateProductCommandHandler(
            IProductRepository productRepository,
            IApplicationDbContext context,
            IDomainEventService domainEventService)
        {
            _productRepository = productRepository;
            _context = context;
            _domainEventService = domainEventService;
        }

        public async Task<Result<Guid>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var productName = new ProductName(request.Name);
                var price = new Money(request.Price, request.Currency);

                var product = new Product(
                    productName,
                    request.Description,
                    price,
                    request.StockQuantity,
                    request.CategoryId);

                await _productRepository.AddAsync(product, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                // Publish domain events
                foreach (var domainEvent in product.DomainEvents)
                {
                    await _domainEventService.PublishAsync(domainEvent, cancellationToken);
                }

                return Result.Success(product.Id);
            }
            catch (Exception ex)
            {
                return Result.Failure<Guid>($"Failed to create product: {ex.Message}");
            }
        }
    }
}
