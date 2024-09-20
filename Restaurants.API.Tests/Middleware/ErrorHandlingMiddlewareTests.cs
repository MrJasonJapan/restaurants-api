using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Xunit;

namespace restaurants_api.Middleware.Tests;

public class ErrorHandlingMiddlewareTests
{
    [Fact()]
    public async Task InvokeAsync_WhenNoExceptionThrown_ShouldCallNextDelegateAsync()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var middleware = new ErrorHandlingMiddleware(loggerMock.Object);
        var context = new DefaultHttpContext();
        var nextDelegateMock = new Mock<RequestDelegate>();

        // Action
        await middleware.InvokeAsync(context, nextDelegateMock.Object);

        // Assert
        nextDelegateMock.Verify(next => next.Invoke(context), Times.Once);
    }

    [Fact()]
    public async Task InvokeAsync_WhenNotFountExceptionThrown_ShouldSetStatusCode404()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var middleware = new ErrorHandlingMiddleware(loggerMock.Object);
        var context = new DefaultHttpContext();
        var notFoundException = new NotFoundException(nameof(Restaurant), "1");

        // Action
        await middleware.InvokeAsync(context, _ => throw notFoundException);

        // Assert
        context.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact()]
    public async Task InvokeAsync_WhenForbidExceptionThrown_ShouldSetStatusCode403()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var middleware = new ErrorHandlingMiddleware(loggerMock.Object);
        var context = new DefaultHttpContext();
        var forbidException = new ForbidException();

        // Action
        await middleware.InvokeAsync(context, _ => throw forbidException);

        // Assert
        context.Response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
    }

    [Fact()]
    public async Task InvokeAsync_WhenGenericExceptionThrown_ShouldSetStatusCode500()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var middleware = new ErrorHandlingMiddleware(loggerMock.Object);
        var context = new DefaultHttpContext();
        var exception = new Exception();

        // Action
        await middleware.InvokeAsync(context, _ => throw exception);

        // Assert
        context.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }
}