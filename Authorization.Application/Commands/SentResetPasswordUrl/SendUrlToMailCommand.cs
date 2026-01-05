using MediatR;

namespace Authorization.Application.Commands.SentResetPasswordUrl;

public record SendUrlToMailCommand(string? Email) : IRequest;