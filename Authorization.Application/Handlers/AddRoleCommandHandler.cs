using Authorization.Application.Commands;
using Authorization.Domain.DbModels;
using Authorization.Infrastructure.Interfaces;
using MapsterMapper;
using MediatR;
using Shared.Exceptions;
using Shared.Helpers;

namespace Authorization.Application.Handlers;

public class AddRoleCommandHandler(IRoleRepository roleRepository, IMapper mapper, AuthHelper authHelper)
    : IRequestHandler<AddRoleCommand, bool>
{
    public async Task<bool> Handle(AddRoleCommand request, CancellationToken cancellationToken)
    {
        if (request.RequestRoleDto == null) throw new UserFriendlyException(ErrorMessages.RoleDataRequired);

        if (request.RequestRoleDto.PermittedEndpoints.Any(permittedEndpoint => permittedEndpoint.EndpointId < 1))
            throw new UserFriendlyException(ErrorMessages.PermittedEndpointsIdCantBeNegative);
        
        var domainModel = mapper.Map<Role>(request.RequestRoleDto);

        return await roleRepository.Create(domainModel, authHelper.UserId);
    }
}