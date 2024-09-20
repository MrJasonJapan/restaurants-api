using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Restaurants.API.Controllers.Tests;

public class RestaurantsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    // The term "factory" is used because WebApplicationFactory<Program> sets up a test server 
    // that mimics the behavior of our actual API application. It effectively "manufactures" 
    // instances of our API for integration testing purposes.
    // The factory will also allow us to create a new HttpClient for each test, which is useful for making requests to the API.
    // * The term "factory" in WebApplicationFactory<Program> is intentionally chosen to reflect its role in
    // * manufacturing both the test server and the HttpClient instances needed for integration testing.
    private readonly WebApplicationFactory<Program> _factory;

    public RestaurantsControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact()]
    public async Task GetAll_ForValidRequest_Returns200OkAsync()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var result = await client.GetAsync("/api/restaurants?pageNumber=1&pageSize=10");

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    // get all for invalid request return 400 BadRequest
    [Fact]
    public async Task GetAll_ForInvalidRequest_Returns400BadRequestAsync()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var result = await client.GetAsync("/api/restaurants");

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);        
    }   
}