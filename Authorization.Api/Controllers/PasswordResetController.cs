using Authorization.Application.Commands.SentResetPasswordUrl;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO;

namespace Authorization.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PasswordResetController(IMediator mediator) : ControllerBase
{
    [HttpPost("ResetLink")]
    public async Task<IActionResult> ResetLink(string? email)
    {
        await mediator.Send(new SendUrlToMailCommand(email));
        return Ok(new { message = "Reset link has been sent." });
    }
    [HttpPost("ResetPassword")]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPassword)
    {
        await mediator.Send(new ResetPasswordCommand(resetPassword));
        return new JsonResult("Ok");
    }
}
