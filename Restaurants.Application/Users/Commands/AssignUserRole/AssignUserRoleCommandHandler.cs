using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users.UpdateUserDetails.Commands;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Application.Users.Commands.AssignUserRole;

public class AssignUserRoleCommandHandler(
    ILogger<AssignUserRoleCommandHandler> logger,
    UserManager<User> userManager,
    RoleManager<IdentityRole> roleManager) : IRequestHandler<AssignUserRoleCommand>
{
    public async Task Handle(AssignUserRoleCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Assigning user role: {@Request}", request);

        var user = await userManager.FindByEmailAsync(request.UserEmail) 
            ?? throw new NotFoundException(nameof(User), request.UserEmail);

        var role = await roleManager.FindByNameAsync(request.RoleName) 
            ?? throw new NotFoundException(nameof(IdentityRole), request.RoleName);

        // note: the userManager won't throw an exception if the user is already in the role.
        await userManager.AddToRoleAsync(user, role.Name!);

        // memo: temp db fix to solve issue where tole name does not match the normalized name. 
        // todo: delete this memo later.
        //  Update AspNetRoles
        //  Set NormalizedName = UPPER(Name)
    }
}