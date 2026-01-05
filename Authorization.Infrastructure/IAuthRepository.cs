using Authorization.Domain.DbModels;
using Authorization.Domain.Dto;

namespace Authorization.Infrastructure;

public interface IAuthRepository
{
    Task<User> GetUser(string username);
    Task<bool> GetUserBool(string email);
    Task<string?> UserExists(string email);
    Task<User?> GetUserById(int userId);
    Task<IEnumerable<UserWithRoleDto>> GetAllUsersWithRoles(int page, int amount, string? name, string? username, 
        DateTime? fromDate, DateTime? toDate, int? roleId, int? groupId, bool? status);
    Task SignUpUser(User user);
    Task ResetPassword(string password,string token);
    Task UpdateUser(User user);
    Task DeleteUser(int userId);
    Task SaveRefreshToken(int userId, string token, string refreshToken);
    Task<string?> GetRefreshToken(int userId);
}