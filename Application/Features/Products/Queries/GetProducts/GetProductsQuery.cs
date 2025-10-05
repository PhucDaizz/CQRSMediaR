using MediatR;

namespace Application.Features.Products.Queries.GetProducts
{
    public record GetProductsQuery : IRequest<List<ProductDto>>
    {
        public Guid? CategoryId { get; init; }
        public bool? IsActive { get; init; }
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }

    public record ProductDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public decimal Price { get; init; }
        public string Currency { get; init; } = string.Empty;
        public int StockQuantity { get; init; }
        public bool IsActive { get; init; }
        public string CategoryName { get; init; } = string.Empty;
        public DateTime CreatedAt { get; init; }
    }
}
