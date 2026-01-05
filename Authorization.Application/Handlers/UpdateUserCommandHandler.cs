using Authorization.Application.Commands;
using Authorization.Domain.DbModels;
using Authorization.Infrastructure;
using MapsterMapper;
using MediatR;
using Shared.Exceptions;

namespace Authorization.Application.Handlers;

public class UpdateUserCommandHandler(
    IAuthRepository authRepository,
    IMapper mapper)
    : IRequestHandler<UpdateUserCommand, Unit>
{
    public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await authRepository.GetUserById(request.User.Id);
        if (existingUser == null) throw new UserFriendlyException(ErrorMessages.UserNotFound);
        if (existingUser.IsDeleted == true) throw new UserFriendlyException(ErrorMessages.UserIsDeleted);
        
        var updatedUser = existingUser with
        {
            FirstName = request.User.FirstName,
            LastName = request.User.LastName,
            Email = request.User.Email,
            PhoneNumber = request.User.PhoneNumber,
            TagIds = request.User.TagIds,
        };

        var mappedUser = mapper.Map<User>(updatedUser);

        await authRepository.UpdateUser(mappedUser);

        return Unit.Value;
    }
}