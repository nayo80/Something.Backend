using Authorization.Domain.DbModels;

namespace Authorization.Infrastructure.Interfaces;

public interface IRoleRepository
{
    Task<(IEnumerable<Role>, int)> GetAll(int page, int amount);
    Task<IEnumerable<RolesCounts>> GetRolesCounts();
    Task<bool> Create(Role role, int userId);
    Task<bool> Edit(Role role);
    Task<bool> Delete(int id);
}