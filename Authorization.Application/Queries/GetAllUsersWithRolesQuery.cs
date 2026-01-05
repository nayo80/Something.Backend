using Authorization.Domain.Dto;
using MediatR;

namespace Authorization.Application.Queries;

public record GetAllUsersWithRolesQuery(int Page, int Amount, string? Name, string? Username, 
    DateTime? FromDate, DateTime? ToDate, int? RoleId, int? GroupId, bool? Status) : IRequest<IEnumerable<UserWithRoleDto>>;