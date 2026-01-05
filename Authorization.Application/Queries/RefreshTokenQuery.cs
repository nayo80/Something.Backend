using Authorization.Application.DTO;
using MediatR;

namespace Authorization.Application.Queries;

public record RefreshTokenQuery(string ExpiredToken, string RefreshToken) : IRequest<SignInDto>;