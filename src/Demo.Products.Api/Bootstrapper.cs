using Demo.Products.Api.Features.AddProduct;

namespace Demo.Products.Api;

public static class Bootstrapper
{
    public static void RegisterApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<ICreateProductService, CreateProductService>();
    }
}