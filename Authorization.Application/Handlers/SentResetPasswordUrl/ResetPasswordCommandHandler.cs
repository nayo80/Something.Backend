using Authorization.Application.Commands.SentResetPasswordUrl;
using Authorization.Infrastructure;
using MediatR;
using Shared.Exceptions;
using Shared.Guards;

namespace Authorization.Application.Handlers.SentResetPasswordUrl;

public class ResetPasswordCommandHandler(IAuthRepository repository) : IRequestHandler<ResetPasswordCommand>
{
    public async Task Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        Guards.NotNull(request.ResetPassword?.Password,nameof(request.ResetPassword.Password),"Reset password is null");
        if (!request.ResetPassword.IsValidPassword()) throw new UserFriendlyException(ErrorMessages.InvalidPassword);
        string hashPassword = PasswordService.HashPassword(request.ResetPassword.Password);
        await repository.ResetPassword(hashPassword, request.ResetPassword.Token);
    }
}