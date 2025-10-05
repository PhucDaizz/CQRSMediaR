using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Features.Products.Queries.GetProducts;
using Domain.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IProductRepository productRepository, ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task<Result<List<ProductDto>>> GetProductsByCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default)
        {
            try
            {
                var products = await _productRepository.GetByCategoryAsync(categoryId, cancellationToken);

                var productDtos = products.Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name.Value,
                    Description = p.Description,
                    Price = p.Price.Amount,
                    Currency = p.Price.Currency,
                    StockQuantity = p.StockQuantity,
                    IsActive = p.IsActive,
                    CategoryName = p.Category.Name,
                    CreatedAt = p.CreatedAt
                }).ToList();

                return Result.Success(productDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting products by category {CategoryId}", categoryId);
                return Result.Failure<List<ProductDto>>($"Error getting products: {ex.Message}");
            }
        }

        public async Task<Result<ProductDto?>> GetProductByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(id, cancellationToken);

                if (product == null)
                    return Result.Success<ProductDto?>(null);

                var productDto = new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name.Value,
                    Description = product.Description,
                    Price = product.Price.Amount,
                    Currency = product.Price.Currency,
                    StockQuantity = product.StockQuantity,
                    IsActive = product.IsActive,
                    CategoryName = product.Category.Name,
                    CreatedAt = product.CreatedAt
                };

                return Result.Success<ProductDto?>(productDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting product by id {ProductId}", id);
                return Result.Failure<ProductDto?>($"Error getting product: {ex.Message}");
            }
        }

        public async Task<Result> CheckStockAvailabilityAsync(Guid productId, int requiredQuantity, CancellationToken cancellationToken = default)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(productId, cancellationToken);

                if (product == null)
                    return Result.Failure("Product not found");

                if (!product.IsActive)
                    return Result.Failure("Product is not active");

                if (product.StockQuantity < requiredQuantity)
                    return Result.Failure($"Insufficient stock. Available: {product.StockQuantity}, Required: {requiredQuantity}");

                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking stock availability for product {ProductId}", productId);
                return Result.Failure($"Error checking stock: {ex.Message}");
            }
        }

        public async Task<Result> UpdateStockAsync(Guid productId, int newQuantity, CancellationToken cancellationToken = default)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(productId, cancellationToken);

                if (product == null)
                    return Result.Failure("Product not found");

                product.UpdateStock(newQuantity);
                await _productRepository.UpdateAsync(product, cancellationToken);

                _logger.LogInformation("Updated stock for product {ProductId} to {NewQuantity}", productId, newQuantity);
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating stock for product {ProductId}", productId);
                return Result.Failure($"Error updating stock: {ex.Message}");
            }
        }
    }
}
