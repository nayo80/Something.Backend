using Authorization.Application.Commands;
using Authorization.Infrastructure;
using MapsterMapper;
using MediatR;
using Shared.Exceptions;

namespace Authorization.Application.Handlers;

public class SignUpCommandHandler(IAuthRepository authRepository, IMapper mapper)
    : IRequestHandler<SignUpCommand>
{
    public async Task Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        // if (!request.User.IsValidPassword()) throw new UserFriendlyException(ErrorMessages.InvalidPassword);
        var passwordHash = PasswordService.HashPassword(request.User.Password);
        request.User = request.User with { Password = passwordHash};
        var mappedUser = mapper.Map<Domain.DbModels.User>(request.User);

        await authRepository.SignUpUser(mappedUser);
    }
}