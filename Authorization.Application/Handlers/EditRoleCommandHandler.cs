using Authorization.Application.Commands;
using Authorization.Domain.DbModels;
using Authorization.Infrastructure.Interfaces;
using MapsterMapper;
using MediatR;
using Shared.Helpers;

namespace Authorization.Application.Handlers;

public class EditRoleCommandHandler(IMapper mapper, IRoleRepository roleRepository)
    : IRequestHandler<EditRoleCommand, bool>
{
    public async Task<bool> Handle(EditRoleCommand request, CancellationToken cancellationToken)
    {
        var domainModel = mapper.Map<Role>(request.UpdateRoleDto);

        return await roleRepository.Edit(domainModel);
    }
}