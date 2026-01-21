using System.Data;
using Authorization.Domain.DbModels;
using Dapper;
using Shared.Exceptions;

namespace Authorization.Infrastructure;

public class AuthRepository(IDbConnection connection) : IAuthRepository
{
    public async Task<User> GetUser(string email)
    {
        var user = await connection.QuerySingleOrDefaultAsync<User>("[dbo].[GetUser]", new
        {
            Email = email
        }, commandType: CommandType.StoredProcedure);


        if (user == null) throw new UserFriendlyException(ErrorMessages.AuthorizationFailedCheckCredentials);

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
        var user = await connection.QuerySingleOrDefaultAsync("[dbo].[GetUserById]", new
        {
            Id = userId
        }, commandType: CommandType.StoredProcedure);

        if (user == null) throw new UserFriendlyException(ErrorMessages.AuthNotPermitted);

        return user;
    }

    public async Task SignUpUser(User user)
    {
        await connection.ExecuteAsync("[dbo].[SignUpUser]", new
        {
            user.Password,
            user.FirstName,
            user.LastName,
            user.Email,
            user.PhoneNumber,
            user.RoleId
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
        await connection.ExecuteAsync("[dbo].[UpdateUser]", new
        {
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            user.PhoneNumber
        }, commandType: CommandType.StoredProcedure);
    }

    public async Task DeleteUser(int userId)
    {
        await connection.ExecuteAsync("[dbo].[DeleteUser]", new
        {
            Id = userId
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