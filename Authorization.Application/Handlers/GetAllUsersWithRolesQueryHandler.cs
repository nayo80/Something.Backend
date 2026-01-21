// using Authorization.Application.Queries;
// using Authorization.Domain.Dto;
// using Authorization.Infrastructure;
// using MediatR;
//
// namespace Authorization.Application.Handlers;
//
// public class GetAllUsersWithRolesQueryHandler(IAuthRepository authRepository)
//     : IRequestHandler<GetAllUsersWithRolesQuery, IEnumerable<UserWithRoleDto>>
// {
//     public async Task<IEnumerable<UserWithRoleDto>> Handle(GetAllUsersWithRolesQuery request,
//         CancellationToken cancellationToken)
//     {
//         var users = await authRepository.GetAllUsersWithRoles(request.Page, request.Amount,
//             request.Name,
//             request.Username,
//             request.FromDate,
//             request.ToDate,
//             request.GroupId,
//             request.RoleId,
//             request.Status);
//         return users;
//     }
// }