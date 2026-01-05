using Authorization.Application.Commands.SentResetPasswordUrl;
using Authorization.Infrastructure;
using MediatR;
using Microsoft.Extensions.Configuration;
using Shared.Guards;
using Shared.Helpers;

namespace Authorization.Application.Handlers.SentResetPasswordUrl;

public class SendUrlToMailCommandHandler(IConfiguration config,IEmailSender emailSender,IAuthRepository authRepository) :IRequestHandler<SendUrlToMailCommand>
{
    public async Task Handle(SendUrlToMailCommand request, CancellationToken cancellationToken)
    {
        Guards.NotNull(request.Email,nameof(request.Email),"Email is Null");
        string? userExists = await authRepository.UserExists(request.Email);
        Guards.NotNull(userExists,nameof(userExists),"If the email is registered, a reset link has been sent.");
        var baseUrl = config["MailSettings:FromName"]?.TrimEnd('/');
        var resetUrl = $"{baseUrl}?token={userExists}";
        await emailSender.SendResetLinkAsync(request.Email, resetUrl);
    }
}