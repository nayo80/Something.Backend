using Authorization.Application.DTO;
using MediatR;

namespace Authorization.Application.Commands;

public record UpdateUserCommand(UpdateUserDto User) : IRequest<Unit>;