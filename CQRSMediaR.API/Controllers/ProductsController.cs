using Application.Common.Interfaces;
using Application.Features.Products.Commands.CreateProduct;
using Application.Features.Products.Commands.UpdateProductStock;
using Application.Features.Products.Queries.GetProducts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CQRSMediatR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IProductService _productService;

        public ProductsController(IMediator mediator, IProductService productService)
        {
            _mediator = mediator;
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductDto>>> GetProducts([FromQuery] GetProductsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(Guid id)
        {
            var result = await _productService.GetProductByIdAsync(id);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            if (result.Value == null)
                return NotFound();

            return Ok(result.Value);
        }

        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<List<ProductDto>>> GetProductsByCategory(Guid categoryId)
        {
            var result = await _productService.GetProductsByCategoryAsync(categoryId);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateProduct(CreateProductCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return CreatedAtAction(nameof(GetProduct), new { id = result.Value }, result.Value);
        }

        [HttpPut("{id}/stock")]
        public async Task<ActionResult> UpdateStock(Guid id, UpdateProductStockCommand command)
        {
            if (id != command.ProductId)
                return BadRequest("ID mismatch");

            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return NoContent();
        }

        [HttpPost("{id}/check-stock")]
        public async Task<ActionResult> CheckStock(Guid id, [FromBody] int requiredQuantity)
        {
            var result = await _productService.CheckStockAvailabilityAsync(id, requiredQuantity);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return Ok(new { Available = true });
        }
    }
}
