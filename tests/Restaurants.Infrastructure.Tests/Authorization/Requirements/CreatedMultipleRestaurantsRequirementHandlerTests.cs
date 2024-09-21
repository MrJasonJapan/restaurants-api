using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using restaurants_api.Application.Users;
using Xunit;

namespace Restaurants.Infrastructure.Authorization.Requirements.Tests;

public class CreatedMultipleRestaurantsRequirementHandlerTests
{
    [Fact]
    public async Task HandleRequirementAsync_UserHasCreatedMultipleRestaurants_ShouldSucceedAsync()
    {
        // Arrange
        var currentUser = new CurrentUser("1", "test@test.com", new List<string>(), null, null);
        
        // Mock the IUserContext to return the currentUser when GetCurrentUser is called
        var userContextMock = new Mock<IUserContext>();
        userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);

        // Create a list of restaurants:
        // - Two owned by currentUser
        // - One owned by another user (Id "2")
        var restaurants = new List<Restaurant>
        {
            new Restaurant { OwnerId = currentUser.Id },
            new Restaurant { OwnerId = currentUser.Id },
            new Restaurant { OwnerId = "2" }
        };

        // Mock the IRestaurantsRepository to return the above list when GetAll is called
        var restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();
        restaurantsRepositoryMock.Setup(r => r.GetAll()).ReturnsAsync(restaurants);

        // Define the requirement: user must have created at least 2 restaurants
        var requirement = new CreatedMultipleRestaurantsRequirement(2);
        
        // Instantiate the handler with the mocked dependencies
        var handler = new CreatedMultipleRestaurantsRequirementHandler(restaurantsRepositoryMock.Object, userContextMock.Object);

        // Creating AuthorizationHandlerContext with:
        // - The specific requirement to evaluate.
        // - null for User because the handler retrieves the current user via IUserContext.
        // └ in other words, because we have the userContextMock defined above, we don't need to pass in a user. this would be redundant.
        // - null for Resource as this requirement doesn't pertain to a specific resource.
        // This setup isolates the handler's logic, focusing solely on whether the user meets the restaurant creation criteria.
        var context = new AuthorizationHandlerContext(new[] { requirement }, null, null);

        // Act
        // Invoke the handler's HandleAsync method to process the authorization
        await handler.HandleAsync(context);

        // Assert
        // Verify that the authorization succeeded
        context.HasSucceeded.Should().BeTrue();
    }

    [Fact]
    public async Task HandleRequirementAsync_UserHasNotCreatedMultipleRestaurants_ShouldFailAsync()
    {
        // Arrange
        var currentUser = new CurrentUser("1", "test@test.com", new List<string>(), null, null);
        var restaurants = new List<Restaurant>
        {
            new Restaurant { OwnerId = currentUser.Id },
            new Restaurant { OwnerId = "2" }
        };

        var userContextMock = new Mock<IUserContext>();
        userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);

        var restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();
        restaurantsRepositoryMock.Setup(r => r.GetAll()).ReturnsAsync(restaurants);

        var requirement = new CreatedMultipleRestaurantsRequirement(2);

        var handler = new CreatedMultipleRestaurantsRequirementHandler(restaurantsRepositoryMock.Object, userContextMock.Object);

        var context = new AuthorizationHandlerContext(new[] { requirement }, null, null);

        // Act
        await handler.HandleAsync(context);

        // Assert
        context.HasFailed.Should().BeTrue();
    }
}