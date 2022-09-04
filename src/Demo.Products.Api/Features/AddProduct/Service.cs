using LanguageExt;
using LanguageExt.Common;
using static LanguageExt.Prelude;

namespace Demo.Products.Api.Features.AddProduct;

public interface ICreateProductService
{
    Aff<Unit> ExecuteAsync(Request request);
}

public class CreateProductService : ICreateProductService
{
    public Aff<Unit> ExecuteAsync(Request request) =>
        from op in AffMaybe<Unit>(async () => await AddProductAsync(request))
        select op;

    private static async Task<Unit> AddProductAsync(Request request)
    {
        // TODO:
        await Task.Delay(TimeSpan.FromSeconds(2));
        return unit;
    }
}
