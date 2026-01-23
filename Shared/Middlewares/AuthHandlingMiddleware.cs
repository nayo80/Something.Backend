using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shared.Exceptions;

namespace Shared.Middlewares;

public partial class AuthHandlingMiddleware : IMiddleware
{
    private readonly IDbConnection _connection;
    private readonly JwtSecurityTokenHandler _handler;
    private readonly TokenValidationParameters _validationParameters;

    public AuthHandlingMiddleware(IConfiguration configuration, IDbConnection connection)
    {
        _connection = connection;

        _handler = new JwtSecurityTokenHandler();

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

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
            try
            {
                var principal = _handler.ValidateToken(token, _validationParameters, out _);
                var claim = _handler.ReadJwtToken(token)
                    .Claims
                    .FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);

                if (claim == null) throw new UserFriendlyException(ErrorMessages.AuthNotPermitted);

                var apiPattern = UserPathRegex();

                var path = context.Request.Path.Value;

                if (path == null) throw new UserFriendlyException(ErrorMessages.AuthNotPermitted);

                var userPath = apiPattern
                    .Match(path)
                    .Value;
                var userAllowed = await _connection
                    .QueryFirstOrDefaultAsync<bool>("dbo.CheckUserPermissions", new
                    {
                        UserId = claim.Value,
                        Endpoint = userPath,
                        context.Request.Method
                    });


                if (!userAllowed)
                    throw new UserFriendlyException(ErrorMessages.UserNotEnoughPermissions);

                context.User = principal;
            }
            catch
            {
                throw new UserFriendlyException(ErrorMessages.UserNotEnoughPermissions);
                // return;
            }

        await next(context);
    }


    [GeneratedRegex(@"/api/[^\s/]+")]
    private static partial Regex UserPathRegex();
}