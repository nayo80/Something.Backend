using Authorization.Application.DTO.RoleDTOs;
using MediatR;

namespace Authorization.Application.Commands;

public record AddRoleCommand(RequestRoleDto RequestRoleDto) : IRequest<bool>;