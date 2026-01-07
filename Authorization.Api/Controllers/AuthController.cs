using Authorization.Application.Commands;
using Authorization.Application.DTO;
using Authorization.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Exceptions;

namespace Authorization.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator,ILogger<AuthController>logger) : ControllerBase
{
    [HttpGet(template: "signin")]
    public async Task<IActionResult> SignIn(string email, string password)
    {
        var token = await mediator.Send(request: new SignInQuery(Email: email, Password: password));
        var date = DateTime.UtcNow;
        // if (!string.IsNullOrEmpty(token.Token))
        // {
        //     logger.LogInformation($"User {email} successfully logged in at {date}.");
        // }
        
        return new JsonResult(value: token);
    }

    [HttpGet("{refreshToken}")]
    public async Task<IActionResult> RefreshToken(string refreshToken)
    {
        if (!Request.Headers.TryGetValue("Authorization", out var authHeader))
            throw new UserFriendlyException(ErrorMessages.AuthTokenInvalid);

        var token = authHeader.ToString()["Bearer ".Length..].Trim();
        var newToken = await mediator.Send(new RefreshTokenQuery(token, refreshToken));

        return new JsonResult(newToken);
    }

    [HttpGet("users/{page:int}/{amount:int}")]
    public async Task<IActionResult> GetAllUsersWithRoles(int page, int amount, string? name, string? username,
        DateTime? fromDate, DateTime? toDate, int? roleId, int? groupId, bool? status)
    {
        var resp = await mediator.Send(
            new GetAllUsersWithRolesQuery(page, amount, name, username, fromDate, toDate, roleId, groupId, status));

        return new JsonResult(resp);
    }

    [HttpPost]
    public async Task<IActionResult> SignUp([FromBody] SignUpDto user)
    {
        await mediator.Send(new SignUpCommand(user));


        return new JsonResult("Ok");
    }


    [HttpPut]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto userDto)
    {
        await mediator.Send(new UpdateUserCommand(userDto));


        return new JsonResult("Updated Successfully");
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        await mediator.Send(new DeleteUserCommand(id));


        return new JsonResult("Deleted Successfully");
    }
}
