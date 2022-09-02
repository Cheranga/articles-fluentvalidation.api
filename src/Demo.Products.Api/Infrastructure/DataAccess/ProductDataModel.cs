namespace Demo.Products.Api.Infrastructure.DataAccess;

public record struct ProductDataModel(
    string CorrelationId,
    string ProductId,
    string ProductName,
    DateTime UpdatedDateTime);