// using Authorization.Application.DTO;
// using Authorization.Application.Queries;
// using Authorization.Infrastructure;
// using MediatR;
// using Shared.Exceptions;
//
// namespace Authorization.Application.Handlers;
//
// public class SignInGoogleUserQueryHandler(
//     TokenService tokenService,
//     IAuthRepository authRepository)
//     : IRequestHandler<SignInGoogleUserQuery, SignInDto>
// {
//     public async Task<SignInDto> Handle(SignInGoogleUserQuery request, CancellationToken cancellationToken)
//     {
//         var userAndRestaurant = await authRepository.GetUser(request.Email);
//         if (userAndRestaurant.User.IsDeleted) throw new UserFriendlyException(ErrorMessages.AuthPermitUserIsDeleted);
//
//         var token = tokenService.GenerateJwtToken(userAndRestaurant);
//         var refreshToken = TokenService.GenerateRefreshToken();
//
//         await authRepository.SaveRefreshToken(userAndRestaurant.User.Id, token, refreshToken);
//
//         return new SignInDto
//         {
//             Token = token,
//             RefreshToken = refreshToken
//         };
//     }
// }