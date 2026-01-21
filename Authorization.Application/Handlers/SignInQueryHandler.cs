using Authorization.Application.DTO;
using Authorization.Application.Queries;
using Authorization.Infrastructure;
using MediatR;
using Shared.Exceptions;

namespace Authorization.Application.Handlers;

public class SignInQueryHandler(
    TokenService tokenService,
    IAuthRepository authRepository)
    : IRequestHandler<SignInQuery, SignInDto>
{
    public async Task<SignInDto> Handle(SignInQuery request, CancellationToken cancellationToken)
    {
        var user = await authRepository.GetUser(request.Email);
        if (user.IsDeleted) throw new UserFriendlyException(ErrorMessages.AuthPermitUserIsDeleted);
        var isOkay = PasswordService.VerifyPassword(request.Password, user.Password);

        if (!isOkay) throw new UserFriendlyException(ErrorMessages.AuthNotPermitted);

        
        var token = tokenService.GenerateJwtToken(user);
        var refreshToken = TokenService.GenerateRefreshToken();

        await authRepository.SaveRefreshToken(user.Id, token, refreshToken);

        return new SignInDto
        {
            Token = token,
            RefreshToken = refreshToken
        };
    }
}