using Microsoft.AspNetCore.Http;

namespace Shared.Middlewares;

public class AppKeyHandlingMiddleware : IMiddleware
{
    private const string ExpectedKey = "EI26n7sAPn0NaJJ";

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.Request.Headers.TryGetValue("ApplicationKey", out var appKey))
        {
            //when app key found but not equal
            if (appKey != ExpectedKey)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Something went wrong");
                return;
            }
        }
        else
        {
            //when app key header doesn't exist
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("Something went wrong");
            return;
        }

        await next(context);
    }
}