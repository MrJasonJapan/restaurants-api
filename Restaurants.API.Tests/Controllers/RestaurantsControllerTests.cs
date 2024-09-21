using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Restaurants.API.Tests;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
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
    private readonly Mock<IRestaurantsRepository> _restaurantsRepositoryMock = new();

    public RestaurantsControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                // Replace the actual repository with the mock repository
                services.Replace(ServiceDescriptor.Scoped<IRestaurantsRepository>(_ => _restaurantsRepositoryMock.Object));
            });
        });
    }

    // getById for existing id should return 200 OK, with the correct data
    [Fact]
    public async Task GetById_ForExistingId_ShouldReturn200OK()
    {
        // Arrange
        var id = 99;

        var restaurant = new Restaurant { Id = id, Name = "Test Restaurant", Description = "Test Description" };

        _restaurantsRepositoryMock.Setup(x => x.GetById(id)).ReturnsAsync(restaurant);

        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync($"/api/restaurants/{id}");
        var restaurantDto = await response.Content.ReadFromJsonAsync<RestaurantDto>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        restaurantDto.Should().NotBeNull();
        restaurantDto.Name.Should().Be("Test Restaurant");
        restaurantDto.Description.Should().Be("Test Description");
    }

    [Fact()]
    public async Task GetById_ForNonExistingId_ShouldReturn404NotFound()
    {
        // Arrange
        var id = 999;

        // Regardless of the executing machine, the test will now always return the same result.
        _restaurantsRepositoryMock.Setup(x => x.GetById(id)).ReturnsAsync((Restaurant?)null);    

        var client = _factory.CreateClient();

        // Act
        var result = await client.GetAsync($"/api/restaurants/{id}");

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact()]
    public async Task GetAll_ForValidRequest_ShouldReturn200Ok()
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
    public async Task GetAll_ForInvalidRequest_ShouldReturn400BadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var result = await client.GetAsync("/api/restaurants");

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);        
    }   
}