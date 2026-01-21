using Authorization.Application.DTO.RoleDTOs;
using Authorization.Application.Queries;
using Authorization.Infrastructure.Interfaces;
using MapsterMapper;
using MediatR;
using System.Collections;
using Mapster;

namespace Authorization.Application.Handlers;

public class GetAllRolesQueryHandler(IMapper mapper, IRoleRepository roleRepository)
    : IRequestHandler<GetAllRolesQuery, PagedRolesResponse>
{
    public async Task<PagedRolesResponse> Handle(GetAllRolesQuery request,
        CancellationToken cancellationToken)
    {
        var allRoles = await roleRepository.GetAll(request.Page, request.Amount);
        var mapped = mapper.Map<IEnumerable<ResponseRoleDto>>(allRoles.Item1);
        // ესე დამეპვა ჯობია
        var t = allRoles.Item1.Adapt<IEnumerable<ResponseRoleDto>>();
        var rolesResponse = new PagedRolesResponse()
        {
            Roles = mapped, 
            TotalAmount = allRoles.Item2, 
            Page = request.Page, 
            Amount = request.Amount
        };

        return rolesResponse;
    }
}