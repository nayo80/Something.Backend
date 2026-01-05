using MediatR;
using Shared.DTO;

namespace Authorization.Application.Commands.SentResetPasswordUrl;

public record ResetPasswordCommand(ResetPasswordDto? ResetPassword) : IRequest;