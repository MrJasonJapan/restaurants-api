using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;
using Restaurants.Domains.Constants;
using restaurants_api.Application.Users;
using Xunit;

namespace Restaurants.Application.Restaurants.Commands.Tests;

public class CreateRestaurantCommandHandlerTests
{
    [Fact]
    public async Task Handle_ForValidCommand_ShouldReturnCreatedRestaurantIdAsync()
    {
        // Arrange

        // Mock the logger dependency
        var loggerMock = new Mock<ILogger<CreateRestaurantCommandHandler>>();

        // Mock the mapper dependency and set up its behavior
        var mapperMock = new Mock<IMapper>();
        var command = new CreateRestaurantCommand();
        var restaurant = new Restaurant();
        mapperMock.Setup(mapper => mapper.Map<Restaurant>(command)).Returns(restaurant);

        // Mock the repository dependency and set up its behavior
        var restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();
        restaurantsRepositoryMock.Setup(repo => repo.Create(It.IsAny<Restaurant>())).ReturnsAsync(1);

        // Mock the authorization service and set it to authorize the operation
        var restaurantAuthorizationServiceMock = new Mock<IRestaurantAuthorizationService>();
        restaurantAuthorizationServiceMock
            .Setup(service => service.Authorize(It.IsAny<Restaurant>(), ResourceOperation.Create))
            .Returns(true);

        // Mock the user context and set up the current user
        var userContextMock = new Mock<IUserContext>();
        var currentUser = new CurrentUser("owner-id", "test@test.com", new List<string>(), null, null);
        userContextMock.Setup(context => context.GetCurrentUser()).Returns(currentUser);

        // Create an instance of the handler with mocked dependencies
        // Note: We are testing the actual implementation of CreateRestaurantCommandHandler
        // * Testing the Implementation: You are testing the real implementation of CreateRestaurantCommandHandler.
        // * The handler's code is executed as it would be in production.
        // * Mocking Dependencies: By mocking dependencies like ILogger, IMapper, IRestaurantsRepository, IRestaurantAuthorizationService,
        // * and IUserContext, you control their outputs and interactions.
        // * This isolation ensures that any test failures are due to issues in the handler itself, not in external services.
        var commandHandler = new CreateRestaurantCommandHandler(
            loggerMock.Object,
            mapperMock.Object,
            restaurantsRepositoryMock.Object,
            restaurantAuthorizationServiceMock.Object,
            userContextMock.Object
        );

        // Act

        // Call the Handle method to test the handler's logic
        var result = await commandHandler.Handle(command, CancellationToken.None);

        // Assert

        // Verify that the handler returns the expected restaurant ID
        result.Should().Be(1);

        // Verify that the restaurant's OwnerId is set correctly
        restaurant.OwnerId.Should().Be("owner-id");

        // Verify that the repository's Create method was called once with the correct restaurant
        restaurantsRepositoryMock.Verify(repo => repo.Create(restaurant), Times.Once);
    }
}
