using Authorization.Application.Commands;
using Authorization.Application.DTO;
using Authorization.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Exceptions;

namespace Authorization.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpGet(template: "signin")]
    public async Task<IActionResult> SignIn(string email, string password)
    {
        var token = await mediator.Send(request: new SignInQuery(Email: email, Password: password));
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
