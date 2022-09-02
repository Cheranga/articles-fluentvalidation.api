using HybridModelBinding;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Products.Api.Features.GetProductById;

[ApiController]
public class Controller : ControllerBase
{
    private readonly IProductSearchByIdService _service;

    public Controller(IProductSearchByIdService service) => _service = service;

    [HttpGet("api/products", Name = "GetProductById")]
    public async Task<IActionResult> GetProductById([FromHybrid] RequestDto dto)
    {
        var product = await _service.GetAsync(new Request(dto.CorrelationId, dto.ProductId));
        if (string.IsNullOrEmpty(product.ProductId))
        {
            return NotFound();
        }

        return Ok(product);
    }
}
