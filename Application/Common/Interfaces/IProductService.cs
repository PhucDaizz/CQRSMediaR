using Application.Common.Models;
using Application.Features.Products.Queries.GetProducts;

namespace Application.Common.Interfaces
{
    public interface IProductService
    {
        Task<Result<List<ProductDto>>> GetProductsByCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default);
        Task<Result<ProductDto?>> GetProductByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Result> CheckStockAvailabilityAsync(Guid productId, int requiredQuantity, CancellationToken cancellationToken = default);
        Task<Result> UpdateStockAsync(Guid productId, int newQuantity, CancellationToken cancellationToken = default);
    }
}
