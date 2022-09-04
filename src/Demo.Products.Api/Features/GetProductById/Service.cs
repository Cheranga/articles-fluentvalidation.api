using Demo.Products.Api.Infrastructure.DataAccess;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Demo.Products.Api.Features.GetProductById;

public interface IProductSearchByIdService
{
    Aff<ProductDataModel> GetAsync(Request request);
}

public class ProductSearchByIdService : IProductSearchByIdService
{
    private Task<ProductDataModel> GetProductAsync(Request request)
    {
        return Task.FromResult(
            new ProductDataModel(
                request.CorrelationId,
                request.ProductId,
                "keyboard",
                DateTime.UtcNow
            )
        );
    }

    public Aff<ProductDataModel> GetAsync(Request request) =>
        from op in AffMaybe<ProductDataModel>(async () => await GetProductAsync(request))
        select op;
}
