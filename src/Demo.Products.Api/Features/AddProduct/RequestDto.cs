using HybridModelBinding;

namespace Demo.Products.Api.Features.AddProduct;

[HybridBindClass(new []{Source.Header, Source.Body})]
public class RequestDto
{
    [HybridBindProperty(Source.Header, name: "X-Correlation-ID")]
    public string CorrelationId { get; set; }

    public string Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}