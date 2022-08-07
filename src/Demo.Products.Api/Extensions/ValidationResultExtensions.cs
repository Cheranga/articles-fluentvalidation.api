using System.Net;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Products.Api.Extensions;

public static class ValidationResultExtensions
{
    // public static ProblemDetails ToProblemDetails(ValidationResult validationResult)
    // {
    //     if (validationResult.IsValid)
    //     {
    //         return null;
    //     }
    //     var problemDetails = new ProblemDetails
    //     {
    //         Type = "ValidationError",
    //         Detail = "invalid request, please check the error list for more details",
    //         Status = (int) (HttpStatusCode.BadRequest),
    //         Title = "invalid request"
    //     };
    //     
    //     problemDetails.Extensions.Add("errors", validationResult.Errors.ToDictionary(x=>x.PropertyName, x=>x.ErrorMessage));
    //     return problemDetails;
    // }
    
    public static ProblemDetails ToProblemDetails(this IEnumerable<ValidationFailure> validationFailures)
    {
        var errors = validationFailures.ToDictionary(x => x.PropertyName, x => x.ErrorMessage);
        
        var problemDetails = new ProblemDetails
        {
            Type = "ValidationError",
            Detail = "invalid request, please check the error list for more details",
            Status = (int) (HttpStatusCode.BadRequest),
            Title = "invalid request"
        };
        
        problemDetails.Extensions.Add("errors", errors);
        return problemDetails;
    }
}