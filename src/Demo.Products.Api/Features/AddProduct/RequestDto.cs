namespace Demo.Products.Api.Features.AddProduct;

// TODO: check record types
public class AddProductRequestDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}