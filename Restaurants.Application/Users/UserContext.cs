using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using restaurants_api.Application.Users;

namespace Restaurants.Application.Users
{
    public interface IUserContext
    {
        CurrentUser? GetCurrentUser();
    }

    public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
    {
        public CurrentUser? GetCurrentUser()
        {
            var user = httpContextAccessor?.HttpContext?.User;

            if (user == null)
            {
                throw new InvalidOperationException("User context is not present");
            }

            if (user.Identity == null || !user.Identity.IsAuthenticated)
            {
                throw null;
            }

            var userId = user.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var email = user.FindFirst(ClaimTypes.Email)!.Value;
            var roles = user.FindAll(ClaimTypes.Role).Select(c => c.Value);

            return new CurrentUser(userId, email, roles);
        }
    }
}