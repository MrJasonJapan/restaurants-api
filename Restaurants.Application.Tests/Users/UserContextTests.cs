using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Xunit;

namespace Restaurants.Application.Tests.Users;

public class UserContextTests
{
    [Fact]
    public void GetCurrentUser_WhenUserIsAuthenticated_ShouldReturnUser()
    {
        // Arrange
        var dateOfBirth = new DateOnly(1990, 1, 1);
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, "1"),
            new(ClaimTypes.Email, "test@example.com"),
            new(ClaimTypes.Role, UserRoles.Admin),
            new(ClaimTypes.Role, UserRoles.User),
            new("Nationality", "German"),
            new("DateOfBirth", dateOfBirth.ToString("yyyy-MM-dd")),
        };

        var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuth"));
        
        httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext { User = user });

        var userContext = new UserContext(httpContextAccessorMock.Object);

        // Act
        var currentUser = userContext.GetCurrentUser();

        // Assert
        currentUser.Should().NotBeNull();
        currentUser.Id.Should().Be("1");
        currentUser.Email.Should().Be("test@example.com");
        currentUser.Roles.Should().ContainInOrder(UserRoles.Admin, UserRoles.User);
        currentUser.Nationality.Should().Be("German");
        currentUser.DateOfBirth.Should().Be(dateOfBirth);
    }

    [Fact]
    public void GetCurrentUser_WhenUserContextIsNotPresent_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        httpContextAccessorMock.Setup(x => x.HttpContext).Returns((HttpContext?)null);

        var userContext = new UserContext(httpContextAccessorMock.Object);

        // Act
        var action = () => userContext.GetCurrentUser();

        // Assert
        action.Should().Throw<InvalidOperationException>().WithMessage("User context is not present");
    }
}
