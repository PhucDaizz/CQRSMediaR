using Application.Common.Models;
using MediatR;

namespace Application.Features.Products.Commands.UpdateProductStock
{
    public record UpdateProductStockCommand : IRequest<Result>
    {
        public Guid ProductId { get; init; }
        public int NewQuantity { get; init; }
    }
}
