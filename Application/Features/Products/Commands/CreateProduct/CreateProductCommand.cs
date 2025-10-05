using Application.Common.Models;
using MediatR;

namespace Application.Features.Products.Commands.CreateProduct
{
    public record CreateProductCommand : IRequest<Result<Guid>>
    {
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public decimal Price { get; init; }
        public string Currency { get; init; } = "VND";
        public int StockQuantity { get; init; }
        public Guid CategoryId { get; init; }
    }
}
