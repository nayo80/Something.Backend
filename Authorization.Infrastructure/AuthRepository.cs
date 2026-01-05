using System.Data;
using Authorization.Domain.DbModels;
using Authorization.Domain.Dto;
using Dapper;
using Shared.Exceptions;

namespace Authorization.Infrastructure;

public class AuthRepository(IDbConnection connection) : IAuthRepository
{
    public async Task<User> GetUser(string email)
    {
        var user = await connection.QuerySingleAsync<User>("[dbo].[GetUser]", new
        {
            Email = email
        }, commandType: CommandType.StoredProcedure);


        if (user == null) throw new UserFriendlyException(ErrorMessages.AuthNotPermitted);

        return user;
    }

    public async Task<bool> GetUserBool(string email)
    {
        var grid = await connection.QueryMultipleAsync("[dbo].[GetUser]", new
        {
            Email = email
        }, commandType: CommandType.StoredProcedure);

        var user = await grid.ReadSingleOrDefaultAsync<User>();

        return user != null;
    }

    public async Task<string?> UserExists(string email)
    {
        return await connection.ExecuteScalarAsync<string?>("dbo.CheckEmailExists", new { email },
            commandType: CommandType.StoredProcedure);
    }


    public async Task<User?> GetUserById(int userId)
    {
        var grid = await connection.QueryMultipleAsync("[dbo].[GetUserById]", new
        {
            Id = userId
        }, commandType: CommandType.StoredProcedure);

        var user = await grid.ReadSingleOrDefaultAsync<User>();

        if (user == null) throw new UserFriendlyException(ErrorMessages.AuthNotPermitted);

        return user;
    }

    public async Task SignUpUser(User user)
    {
        DataTable tags = CreateTagTable(user.TagIds);
        await connection.ExecuteAsync("[dbo].[SignUpUser]", new
        {
            user.Password,
            user.FirstName,
            user.LastName,
            user.Email,
            user.PhoneNumber,
            user.RoleId,
            TagIds = tags
        }, commandType: CommandType.StoredProcedure);
    }

    public async Task ResetPassword(string password, string token)
    {
        await connection.ExecuteAsync("dbo.ResetUserPassword", new
        {
            password, token
        }, commandType: CommandType.StoredProcedure);
    }


    public async Task UpdateUser(User user)
    {
        DataTable tags = CreateTagTable(user.TagIds);
        await connection.ExecuteAsync("[dbo].[UpdateUser]", new
        {
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            user.PhoneNumber,
            TagIds = tags
        }, commandType: CommandType.StoredProcedure);
    }

    #region Helpers

    private static DataTable CreateTagTable(IEnumerable<int> tagIds)
    {
        var table = new DataTable();
        table.Columns.Add("TagId", typeof(int));

        foreach (var id in tagIds)
            table.Rows.Add(id);

        return table;
    }

    #endregion
    public async Task DeleteUser(int userId)
    {
        await connection.ExecuteAsync("[dbo].[DeleteUser]", new
        {
            Id = userId
        }, commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<UserWithRoleDto>> GetAllUsersWithRoles(int page, int amount, string? name,
        string? username, DateTime? fromDate, DateTime? toDate, int? roleId, int? groupId, bool? status)
    {
        return await connection.QueryAsync<UserWithRoleDto>("[dbo].[SP_GetAllUsersWithRoles]", new
        {
            Page = page,
            Amount = amount,
            Name = name,
            Username = username,
            FromDate = fromDate,
            ToDate = toDate,
            RoleId = roleId,
            GroupId = groupId,
            Status = status
        }, commandType: CommandType.StoredProcedure);
    }


    public async Task SaveRefreshToken(int userId, string token, string refreshToken)
    {
        await connection.ExecuteAsync("[dbo].[SaveRefreshToken]", new
        {
            UserId = userId,
            Token = token,
            RefreshToken = refreshToken
        }, commandType: CommandType.StoredProcedure);
    }

    public async Task<string?> GetRefreshToken(int userId)
    {
        var token = await connection.QueryFirstOrDefaultAsync<string>("[dbo].[SP_GetUserRefreshToken]", new
        {
            UserId = userId
        }, commandType: CommandType.StoredProcedure);

        return token;
    }
}