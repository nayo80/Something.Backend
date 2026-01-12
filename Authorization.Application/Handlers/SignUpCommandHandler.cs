using Authorization.Application.Commands;
using Authorization.Infrastructure;
using MapsterMapper;
using MediatR;
using Shared.Guards;

namespace Authorization.Application.Handlers;

public class SignUpCommandHandler(IAuthRepository authRepository, IMapper mapper)
    : IRequestHandler<SignUpCommand>
{
    public async Task Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        Guards.NotNull(request.User, nameof(request.User));
        var passwordHash = PasswordService.HashPassword(request.User.Password);
        request.User = request.User with { Password = passwordHash};
        var mappedUser = mapper.Map<Domain.DbModels.User>(request.User);

        await authRepository.SignUpUser(mappedUser);
    }
}