using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Shared.Exceptions;

namespace Shared.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next, IHostEnvironment hostEnvironment)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        var response = context.Response;

        var errorResponse = new ErrorResponse();

        switch (exception)
        {
            case UserFriendlyException ex:
                if (ex.Message == nameof(ErrorMessages.UserNotLoggedIn))
                {
                    response.StatusCode = (int)HttpStatusCode.Forbidden;
                    break;
                }
                
                errorResponse.Message = ex.Message;

                break;
            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                errorResponse.Message = hostEnvironment.IsDevelopment() ? exception.Message : "Something went wrong.";
                break;
        }

        var result = JsonSerializer.Serialize(errorResponse);
        await context.Response.WriteAsync(result);
    }
}