using Authorization.Application.DTO;
using MediatR;

namespace Authorization.Application.Queries;

public record SignInQuery(string Email, string Password) : IRequest<SignInDto>;