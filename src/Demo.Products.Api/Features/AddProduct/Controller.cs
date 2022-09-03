using System.Net;
using HybridModelBinding;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Products.Api.Features.AddProduct;

[ApiController]
public class Controller : ControllerBase
{
    private readonly ICreateProductService _service;
    private readonly ILogger<Controller> _logger;

    public Controller(
        ICreateProductService service,
        ILogger<Controller> logger
    )
    {
        _service = service;
        _logger = logger;
    }

    [HttpPost("api/products", Name = "AddProduct")]
    public async Task<IActionResult> AddProduct([FromHybrid] RequestDto request)
    {
        var addProductRequest = new Request(
            request.CorrelationId,
            request.Id,
            request.Name,
            request.Price
        );
        var operation = await _service.ExecuteAsync(addProductRequest).Run();

        return operation.Match<IActionResult>(
            _ => Accepted(),
            error =>
            {
                _logger.LogWarning(
                    "{CorrelationId} adding product failed {@Error}",
                    request.CorrelationId,
                    error
                );
                return StatusCode((int)(HttpStatusCode.InternalServerError));
            }
        );
    }
}
