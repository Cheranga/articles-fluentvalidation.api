using System.Runtime.InteropServices;
using Demo.Products.Api.Core.Filters;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Moq;

namespace Demo.Products.Api.Tests;

public class ApiValidationFilterTests
{
    [Fact]
    public async Task RequestWithValidInput()
    {
        var mockedCustomValidatorFactory = new Mock<ICustomValidatorFactory>();
        mockedCustomValidatorFactory.Setup(x => x.GetValidatorFor(typeof(SampleProduct)))
            .Returns(new SampleProductValidator());

        var mockedActionContext = new ActionContext(
            Mock.Of<HttpContext>(),
            Mock.Of<RouteData>(),
            Mock.Of<ActionDescriptor>(),
            Mock.Of<ModelStateDictionary>()
        );
        
        var actionExecutingContext = new ActionExecutingContext(
            mockedActionContext,
            new List<IFilterMetadata>(),
            new Dictionary<string, object?>
            {
                {"request", new SampleProduct("1", "Keyboard")}
            }, 
            null);

        ActionExecutionDelegate nextAction = () => Task.FromResult(new ActionExecutedContext(
            mockedActionContext,
            new List<IFilterMetadata>(),
            null));

        var filter = new ApiValidationFilter(mockedCustomValidatorFactory.Object);

        await filter.OnActionExecutionAsync(actionExecutingContext, nextAction);

        actionExecutingContext.Result.Should().BeNull();
    }
    
    [Fact]
    public async Task RequestWithInvalidInput()
    {
        var mockedCustomValidatorFactory = new Mock<ICustomValidatorFactory>();
        mockedCustomValidatorFactory.Setup(x => x.GetValidatorFor(typeof(SampleProduct)))
            .Returns(new SampleProductValidator());

        var mockedActionContext = new ActionContext(
            Mock.Of<HttpContext>(),
            Mock.Of<RouteData>(),
            Mock.Of<ActionDescriptor>(),
            Mock.Of<ModelStateDictionary>()
        );
        var actionExecutingContext = new ActionExecutingContext(
            mockedActionContext,
            new List<IFilterMetadata>(),
            new Dictionary<string, object?>
            {
                {"request", new SampleProduct("1", "")}
            }, 
            null);

        ActionExecutionDelegate nextAction = () => Task.FromResult(new ActionExecutedContext(
            mockedActionContext,
            new List<IFilterMetadata>(),
            null));

        var filter = new ApiValidationFilter(mockedCustomValidatorFactory.Object);

        await filter.OnActionExecutionAsync(actionExecutingContext, nextAction);

        actionExecutingContext.Result.Should().BeOfType<BadRequestObjectResult>();
    }
    
    [Fact]
    public async Task RequestWithNullInput()
    {
        var mockedCustomValidatorFactory = new Mock<ICustomValidatorFactory>();
        mockedCustomValidatorFactory.Setup(x => x.GetValidatorFor(typeof(SampleProduct)))
            .Returns(new SampleProductValidator());

        var mockedActionContext = new ActionContext(
            Mock.Of<HttpContext>(),
            Mock.Of<RouteData>(),
            Mock.Of<ActionDescriptor>(),
            Mock.Of<ModelStateDictionary>()
        );

        SampleProduct nullProduct = null;
        
        var actionExecutingContext = new ActionExecutingContext(
            mockedActionContext,
            new List<IFilterMetadata>(),
            new Dictionary<string, object?>
            {
                {"request", nullProduct}
            }, 
            null);

        ActionExecutionDelegate nextAction = () => Task.FromResult(new ActionExecutedContext(
            mockedActionContext,
            new List<IFilterMetadata>(),
            null));

        var filter = new ApiValidationFilter(mockedCustomValidatorFactory.Object);

        await filter.OnActionExecutionAsync(actionExecutingContext, nextAction);

        actionExecutingContext.Result.Should().BeOfType<BadRequestObjectResult>();
    }
}