using System.Net;
using HybridModelBinding;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Products.Api.Features.GetProductById;

[ApiController]
public class Controller : ControllerBase
{
    private readonly IProductSearchByIdService _service;
    private readonly ILogger<Controller> _logger;

    public Controller(IProductSearchByIdService service, ILogger<Controller> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet("api/products", Name = "GetProductById")]
    public async Task<IActionResult> GetProductById([FromHybrid] RequestDto dto)
    {
        var operation = await _service
            .GetAsync(new Request(dto.CorrelationId, dto.ProductId))
            .Run();

        return operation.Match<IActionResult>(
            model => Ok(model),
            error =>
            {
                _logger.LogWarning(
                    "{CorrelationId} getting product by id failed {@Error}",
                    dto.CorrelationId,
                    error
                );
                return StatusCode((int)(HttpStatusCode.InternalServerError));
            }
        );
    }
}
