using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

namespace Application.Features.Products.Commands.UpdateProductStock
{
    public class UpdateProductStockCommandHandler : IRequestHandler<UpdateProductStockCommand, Result>
    {
        private readonly IProductService _productService;
        private readonly IApplicationDbContext _context;

        public UpdateProductStockCommandHandler(IProductService productService, IApplicationDbContext context)
        {
            _productService = productService;
            _context = context;
        }

        public async Task<Result> Handle(UpdateProductStockCommand request, CancellationToken cancellationToken)
        {
            var result = await _productService.UpdateStockAsync(request.ProductId, request.NewQuantity, cancellationToken);

            if (result.IsSuccess)
            {
                await _context.SaveChangesAsync(cancellationToken);
            }

            return result;
        }
    }
}
