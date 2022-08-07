using Demo.Products.Api.Features.AddProduct;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace Demo.Products.Api;

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