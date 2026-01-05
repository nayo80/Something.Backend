using Authorization.Application.DTO.RoleDTOs;
using MediatR;

namespace Authorization.Application.Queries;

public record GetAllRolesQuery(int Page, int Amount) : IRequest<PagedRolesResponse>;