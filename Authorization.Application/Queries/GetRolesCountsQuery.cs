using Authorization.Application.DTO.RoleDTOs;
using MediatR;

namespace Authorization.Application.Queries;

public class GetRolesCountsQuery : IRequest<IEnumerable<RolesCounts>>;