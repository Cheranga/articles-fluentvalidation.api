using System.Net;
using HybridModelBinding;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Products.Api.Features.AddProduct;

[ApiController]
public class CreateProductsController : ControllerBase
{
    private readonly ICreateProductService _service;

    public CreateProductsController(ICreateProductService service)
    {
        _service = service;
    }

    [HttpPost("api/products")]
    public async Task<IActionResult> AddProduct([FromHybrid] RequestDto request)
    {
        var addProductRequest = new Request(request.CorrelationId, request.Id, request.Name, request.Price);
        var status = await _service.ExecuteAsync(addProductRequest);
        return status ? Ok() : StatusCode((int) (HttpStatusCode.InternalServerError));
    }
}