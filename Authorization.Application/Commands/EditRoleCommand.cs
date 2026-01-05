using Authorization.Application.DTO.RoleDTOs;
using MediatR;

namespace Authorization.Application.Commands;

public record EditRoleCommand(UpdateRoleDto UpdateRoleDto) : IRequest<bool>;