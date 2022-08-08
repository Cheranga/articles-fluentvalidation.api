# Using asynchronous validators in ASP.NET

## Context

We came across an interesting problem to fix, when upgrading our APIs to latest .NET and upgrading their respective packages.

Validation is crucial when designing APIs, and we are using the wonderful `FluentValidation.AspNetCore` in our APIs. 
How it seamlessly allow the engineers to define the validations, and integrate with the ASP.NET echo system is a breeze.

Well we thought until we got this nasty looking error below,

![cannot have async validators](images/cannot_have_async_validators.png)

The error clearly states that our validators contain `asynchronous` validations, and the ASP.NET validation pipeline is not asynchronous and hence can't invoke asynchronous rules.

_Yes, it can be debated that validations must be synchronous, but in reality there are many situations where certain validations have to be asynchronous, such as validating with a data store entry or with a web service.
Also it can be the opinion, that such actions should be the responsibility of the core layer(application / domain), but this was existing code, and in our opinion the approach was correct :relaxed:_  


## Considered options

* Move the `async` validations from the validators, and move it to the application or the business layer.
  * Then we will have to create another level of abstraction to be injected into.
  * We will need to change all our existing validators.
  * We will lose the all the "async" validations performed at the time of model binding.


* Implementing a custom action filter 
  * This must be a `IAsyncActionFilter` since we have asynchronous operations.
  * Can use the existing validators without any change.
  * Must be able to find and create the respective `IValidator` for the requested type, and perform the validations.
  * If there were validations, short circuit the ASP.NET pipeline and return `ProblemDetails` as an error response.

We chose to create a custom action filter.

## Demonstrating the problem, with a sample API

This is an API to manage products. For the sake of brevity I have created only a single endpoint which simulates an adding of a product.

* The registration of FluentValidation in ASP.NET
```csharp
public static class Bootstrapper
{
    public static void RegisterDependencies(WebApplicationBuilder builder)
    {
        // register fluent validation as part of the ASP.NET pipeline
        builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly)
            .AddFluentValidationAutoValidation();
        
        builder.Services.AddSingleton<ICreateProductService, CreateProductService>();
    }
}
```
In the `Program.cs` we register the dependencies
```csharp
using Demo.Products.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
Bootstrapper.RegisterDependencies(builder);
...
...
```

* The `Validator` for the DTO
```csharp
public class AddProductRequestDtoValidator : ModelValidatorBase<AddProductRequestDto>
{
    public AddProductRequestDtoValidator()
    {
        RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("id is required");
        RuleFor(x => x.Name).MustAsync(async (s, token) =>
        {
            await Task.Delay(TimeSpan.FromSeconds(1), token);
            return !string.IsNullOrWhiteSpace(s);
        }).WithMessage("name is required");
    }
}
```

* The action method which will expose the endpoint
```csharp
[ApiController]
public class CreateProductsController : ControllerBase
{
    private readonly ICreateProductService _service;

    public CreateProductsController(ICreateProductService service)
    {
        _service = service;
    }

    [HttpPost("api/products")]
    public async Task<IActionResult> AddProduct([FromBody] AddProductRequestDto request)
    {
        var addProductRequest = new AddProductRequest(request.Id, request.Name, request.Price);
        var status = await _service.ExecuteAsync(addProductRequest);
        return status ? Ok() : StatusCode((int) (HttpStatusCode.InternalServerError));
    }
}
```

*Note that I have used another Nuget package called `HybridModelBinding` to bind the model from different model binders and value providers*

When you run this project, and make an HTTP POST request to this endpoint, you'll get the above mentioned error.

## Custom action filter to validate DTOs
```csharp

```
