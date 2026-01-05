using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Shared.Helpers;

public class AuthHelper(IHttpContextAccessor httpContextAccessor)
{
    public int UserId => GetUserId();
    
    public int GetUserId()
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext == null) return -1; 
        
        if (!int.TryParse(httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId))
            return -1;
        return userId;
    }

    public static string? GetUserFullName(ClaimsPrincipal user)
    {
        return user.Claims.FirstOrDefault(c => c.Type == "Name")?.Value;
    }
}
