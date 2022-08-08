namespace Demo.Products.Api.Features.AddProduct;

public record class Request(string CorrelationId, string Id, string Name, decimal Price)
{
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
}