using HybridModelBinding;

namespace Demo.Products.Api.Features.GetProductById;

public enum Availability
{
    Available,
    UnAvailable
}

[HybridBindClass(new []{Source.Header, Source.QueryString})]
public class RequestDto
{
    [HybridBindProperty(Source.Header)]
    public string CorrelationId { get; set; }
    
    [HybridBindProperty(Source.QueryString)]
    public string ProductId { get; set; }

    [HybridBindProperty(Source.QueryString)]
    public Availability Availability { get; set; }
}