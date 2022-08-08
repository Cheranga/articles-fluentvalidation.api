namespace Demo.Products.Api.Features.AddProduct;

public interface ICreateProductService
{
    Task<bool> ExecuteAsync(Request request);
}

public class CreateProductService : ICreateProductService
{
    public async Task<bool> ExecuteAsync(Request request)
    {
        await Task.Delay(TimeSpan.FromSeconds(2));
        return true;
    }
}