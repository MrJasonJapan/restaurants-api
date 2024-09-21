using FluentAssertions;
using Restaurants.Domain.Constants;
using Xunit;

namespace restaurants_api.Application.Users.Tests;

public class CurrentUserTests
{
    // Naming Convention: TestMethodName_Scenario_ExpectedResult
    [Theory] // theory allows us to run the same test with multiple inputs
    [InlineData(UserRoles.Admin)]
    [InlineData(UserRoles.User)]
    public void IsInRole_WithMatchingRole_ShouldReturnTrue(string roleName)
    {
        // arrange
        var currentUser = new CurrentUser(Guid.NewGuid().ToString(), "test@example.com", [UserRoles.Admin, UserRoles.User], null, null);

        // act
        var isInRole = currentUser.IsInRole(roleName);

        // assert
        isInRole.Should().BeTrue();
    }

    [Fact()] // fact is for a single input test
    public void IsInRole_WithNoMatchingRole_ShouldReturnFalse()
    {
        // arrange
        var currentUser = new CurrentUser(Guid.NewGuid().ToString(), "test@example.com", [UserRoles.Admin, UserRoles.User], null, null);

        // act
        var isInRole = currentUser.IsInRole(UserRoles.Owner);

        // assert
        isInRole.Should().BeFalse();
    }

    [Fact()]
    public void IsInRole_WithNoMatchingRoleBecauseOfIncorrectCasing_ShouldReturnFalse()
    {
        // arrange
        var currentUser = new CurrentUser(Guid.NewGuid().ToString(), "test@example.com", [UserRoles.Admin, UserRoles.User], null, null);

        // act
        var isInRole = currentUser.IsInRole(UserRoles.Admin.ToLower()); // ※ IsInRole should be case-sensitive

        // assert
        isInRole.Should().BeFalse();
    }
}