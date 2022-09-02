namespace Demo.Products.Api.Features.GetProductById;

public record struct Request(string CorrelationId, string ProductId);