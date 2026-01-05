using MediatR;

namespace Authorization.Application.Commands;

public record DeleteRoleCommand(int Id) : IRequest<bool>;