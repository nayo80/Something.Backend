using MediatR;

namespace Authorization.Application.Commands;

public record DeleteUserCommand(int UserId) : IRequest<Unit>;