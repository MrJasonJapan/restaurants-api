using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;
using Restaurants.Domains.Constants;
using Xunit;

namespace Restaurants.Application.Restaurants.Commands.Tests;

public class UpdateRestaurantCommandHandlerTests
{
    private readonly Mock<ILogger<UpdateRestaurantCommandHandler>> _loggerMock;
    private readonly Mock<IRestaurantsRepository> _restaurantsRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IRestaurantAuthorizationService> _restaurantAuthorizationServiceMock;

    private readonly UpdateRestaurantCommandHandler _handler;

    public UpdateRestaurantCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<UpdateRestaurantCommandHandler>>();
        _restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();
        _mapperMock = new Mock<IMapper>();
        _restaurantAuthorizationServiceMock = new Mock<IRestaurantAuthorizationService>();

        _handler = new UpdateRestaurantCommandHandler(
            _loggerMock.Object,
            _mapperMock.Object,
            _restaurantAuthorizationServiceMock.Object,
            _restaurantsRepositoryMock.Object);
    }

    // continue here.
    // hande with valid request should update restaurant
    [Fact()]
    public async Task Handle_WithValidRequest_ShouldUpdateRestaurant()
    {
        // Arrange
        var restaurantId = 1;
        var command = new UpdateRestaurantCommand
        {
            Id = restaurantId,
            Name = "Updated Restaurant",
            Description = "Updated Description",
            HasDelivery = true
        };

        var restaurant = new Restaurant
        {
            Id = restaurantId,
            Name = "Original Restaurant",
            Description = "Original Description",
        };

        _restaurantsRepositoryMock.Setup(r => r.GetById(restaurantId)).ReturnsAsync(restaurant);

        _restaurantAuthorizationServiceMock.Setup(r => r.Authorize(restaurant, ResourceOperation.Update)).Returns(true);
        
        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _restaurantsRepositoryMock.Verify(r => r.SaveChanges(), Times.Once);
        _mapperMock.Verify(m => m.Map(command, restaurant), Times.Once);
    }

    // handle with non existing restaurant should throw not found exception
    [Fact()]
    public async Task Handle_WithNonExistingRestaurant_ShouldThrowNotFoundException()
    {
        // Arrange
        var restaurantId = 1;
        var command = new UpdateRestaurantCommand
        {
            Id = restaurantId
        };

        _restaurantsRepositoryMock.Setup(r => r.GetById(restaurantId)).ReturnsAsync((Restaurant?)null);

        // Action
        // We are creating a delegate (function) that represents the asynchronous operation
        // * In unit testing, when you want to assert that a method throws an exception, you need to capture the method call in a delegate(a function reference) without executing it immediately.
        // * This allows you to pass the method into the assertion framework (like FluentAssertions) to verify that it throws the expected exception when invoked.
        // * By wrapping the call in a delegate (Func<Task>), you defer its execution until you're ready to assert on it.
        Func<Task> action = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<NotFoundException>().WithMessage($"Restaurant with identifier {restaurantId} does not exist.");
    }

    // handle with unauthorized access should throw unauthorized access exception
    [Fact()]
    public async Task Handle_WithUnauthorizedAccess_ShouldThrowForbidException()
    {
        // Arrange
        var restaurantId = 1;
        var command = new UpdateRestaurantCommand
        {
            Id = restaurantId
        };

        var restaurant = new Restaurant
        {
            Id = restaurantId
        };

        _restaurantsRepositoryMock.Setup(r => r.GetById(restaurantId)).ReturnsAsync(restaurant);

        _restaurantAuthorizationServiceMock.Setup(r => r.Authorize(restaurant, ResourceOperation.Update)).Returns(false);

        // Act
        Func<Task> action = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<ForbidException>();
    }
}