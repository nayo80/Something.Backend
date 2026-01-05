using System.Security.Claims;
using Authorization.Application.DTO;
using Authorization.Application.Queries;
using Authorization.Infrastructure;
using MediatR;
using Shared.Exceptions;

namespace Authorization.Application.Handlers;

public class RefreshTokenQueryHandler(TokenService tokenService, IAuthRepository authRepository)
    : IRequestHandler<RefreshTokenQuery, SignInDto>
{
    public async Task<SignInDto> Handle(RefreshTokenQuery request, CancellationToken cancellationToken)
    {
        var principal = tokenService.GetPrincipal(request.ExpiredToken);
        var userId = principal.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);

        if (userId == null) throw new UserFriendlyException(ErrorMessages.AuthNotPermitted);

        if (!int.TryParse(userId.Value, out var id)) throw new UserFriendlyException(ErrorMessages.AuthTokenInvalid);

        var refreshToken = await authRepository.GetRefreshToken(id);

        if (refreshToken == null) throw new UserFriendlyException(ErrorMessages.AuthRefreshTokenInvalid);
        if (refreshToken != request.RefreshToken)
            throw new UserFriendlyException(ErrorMessages.AuthRefreshTokenInvalid);

        var user = await authRepository.GetUserById(id);

        if (user is null) throw new UserFriendlyException(ErrorMessages.AuthNotPermitted);

        var token = tokenService.GenerateJwtToken(user);
       
        var newRefreshToken = TokenService.GenerateRefreshToken();

        await authRepository.SaveRefreshToken(id, token, newRefreshToken);

        return new SignInDto
        {
            Token = token,
            RefreshToken = newRefreshToken
        };
    }
}