namespace Demo.Products.Api.Features.AddProduct;

public record class AddProductRequest(string Id, string Name, decimal Price)
{
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
}