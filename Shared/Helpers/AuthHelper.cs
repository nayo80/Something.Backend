using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Shared.Helpers;

public class AuthHelper(IHttpContextAccessor httpContextAccessor)
{
    // ეს კაი ჩემგან წამოიღე მარა პროპერტი იმისთვისაა რო GetUserId() private იყოს და პროპერტი გამოიყენო ყველგან პროსტა :D
    public int UserId => GetUserId();
    
    public int GetUserId()
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext == null) return -1; 
        
        if (!int.TryParse(httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId))
            return -1;
        return userId;
    }
}
