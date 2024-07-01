using MediatR;

namespace Restaurants.Application.Users.UpdateUserDetails.Commands;

public class UpdateUserDetailsCommand : IRequest
{
    public DateOnly? DateOfBirth { get; set; }

    public string? Nationality { get; set; }
}