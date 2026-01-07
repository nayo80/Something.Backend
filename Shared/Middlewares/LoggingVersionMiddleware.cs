using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;


namespace Shared.Middlewares;

public partial class LoggingVersionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly TokenValidationParameters _validationParameters;
    private readonly JwtSecurityTokenHandler _handler;
    private readonly ILogger<LoggingVersionMiddleware> _logger;

    public LoggingVersionMiddleware(RequestDelegate next,IConfiguration configuration,
        ILogger<LoggingVersionMiddleware> logger)
    {
        _next = next;
        _logger = logger;

        var keyString = configuration["Jwt:Key"] ?? "";

        var key = Encoding.ASCII.GetBytes(keyString);

        _validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidAudience = configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ClockSkew = TimeSpan.Zero
        };
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var connection = context.RequestServices.GetRequiredService<IDbConnection>();
        
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (!string.IsNullOrEmpty(token))
        {
            var principal = _handler.ValidateToken(token, _validationParameters, out _);
            var claim = _handler.ReadJwtToken(token)
                .Claims
                .FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);

            var userEmail = await connection
                .QueryFirstOrDefaultAsync<string>("dbo.GetUserByIdForLogs", new
                {
                    UserId = claim?.Value
                });


            var startTime = DateTime.UtcNow;

            _logger.LogInformation(
                "Request: {Method} {Path} from {IpAddress}",
                $"user: {userEmail}",
                context.Request.Method,
                context.Request.Path);

            await _next(context);

            var duration = DateTime.UtcNow - startTime;

            _logger.LogInformation(
                "Response: {Method} {Path} {StatusCode} took {Duration}ms",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                duration.TotalMilliseconds);
        }

        
    }

    [GeneratedRegex(@"/api/[^\s/]+")]
    private static partial Regex UserPathRegex();
}