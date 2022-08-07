namespace Demo.Products.Api.Features.AddProduct;

public interface ICreateProductService
{
    Task<bool> ExecuteAsync(AddProductRequest request);
}

public class CreateProductService : ICreateProductService
{
    public async Task<bool> ExecuteAsync(AddProductRequest request)
    {
        await Task.Delay(TimeSpan.FromSeconds(2));
        return true;
    }
}