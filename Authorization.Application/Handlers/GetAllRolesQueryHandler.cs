using Authorization.Application.DTO.RoleDTOs;
using Authorization.Application.Queries;
using Authorization.Infrastructure.Interfaces;
using MediatR;
using Mapster;

namespace Authorization.Application.Handlers;

public class GetAllRolesQueryHandler(IRoleRepository roleRepository)
    : IRequestHandler<GetAllRolesQuery, PagedRolesResponse>
{
    public async Task<PagedRolesResponse> Handle(GetAllRolesQuery request,
        CancellationToken cancellationToken)
    {
        var allRoles = await roleRepository.GetAll(request.Page, request.Amount);
        var mapped = allRoles.Item1.Adapt<IEnumerable<ResponseRoleDto>>();
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