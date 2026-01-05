using Authorization.Application.DTO;
using MediatR;

namespace Authorization.Application.Queries;

public record SignInGoogleUserQuery(string Email) : IRequest<SignInDto>;