using Demo.Products.Api.Features.AddProduct;
using Demo.Products.Api.Features.GetProductById;

namespace Demo.Products.Api;

public static class Bootstrapper
{
    public static void RegisterApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<ICreateProductService, CreateProductService>();
        builder.Services.AddSingleton<IProductSearchByIdService, ProductSearchByIdService>();
    }
}