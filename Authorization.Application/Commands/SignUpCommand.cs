using Authorization.Application.DTO;
using MediatR;

namespace Authorization.Application.Commands;

public record SignUpCommand(SignUpDto User) : IRequest
{
    public SignUpDto User = User;
}