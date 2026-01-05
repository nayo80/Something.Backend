using Authorization.Application.Commands;
using Authorization.Infrastructure.Interfaces;
using MediatR;

namespace Authorization.Application.Handlers;

public class DeleteRoleCommandHandler(IRoleRepository roleRepository) : IRequestHandler<DeleteRoleCommand, bool>
{
    public async Task<bool> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        return await roleRepository.Delete(request.Id);
    }
}