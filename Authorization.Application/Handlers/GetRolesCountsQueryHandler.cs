using Authorization.Application.DTO.RoleDTOs;
using Authorization.Application.Queries;
using Authorization.Infrastructure.Interfaces;
using MapsterMapper;
using MediatR;

namespace Authorization.Application.Handlers;

public class GetRolesCountsQueryHandler(IRoleRepository roleRepository, IMapper mapper) : IRequestHandler<GetRolesCountsQuery, IEnumerable<RolesCounts>>
{
    public async Task<IEnumerable<RolesCounts>> Handle(GetRolesCountsQuery request, CancellationToken cancellationToken)
    {
        var roles = await roleRepository.GetRolesCounts();
        var result = mapper.Map<IEnumerable<RolesCounts>>(roles);
        return result;
    }
}