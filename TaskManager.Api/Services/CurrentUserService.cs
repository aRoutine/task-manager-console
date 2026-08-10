using System.Security.Claims;
using TaskManager.Interfaces;

namespace TaskManager.Api.Services;

public class CurrentUserService(IHttpContextAccessor _httpContextAccessor) : ICurrentUserService
{
    public int UserId
    {
        get
        {
            string? userId = _httpContextAccessor
            .HttpContext?
            .User
            .FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                throw new UnauthorizedAccessException();

            return int.Parse(userId);
        }
    }
}