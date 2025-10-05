using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace Application.Features.Products.Queries.GetProducts
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<ProductDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetProductsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .AsQueryable();

            if (request.CategoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == request.CategoryId.Value);
            }

            if (request.IsActive.HasValue)
            {
                query = query.Where(p => p.IsActive == request.IsActive.Value);
            }

            var products = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(p => new ProductDto
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
                })
                .ToListAsync(cancellationToken);

            return products;
        }
    }
}
