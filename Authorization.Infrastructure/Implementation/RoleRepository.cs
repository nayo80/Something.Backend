using System.Data;
using Authorization.Domain.DbModels;
using Authorization.Infrastructure.Interfaces;
using Dapper;
using Shared.Exceptions;

namespace Authorization.Infrastructure.Implementation;

public class RoleRepository(IDbConnection connection) : IRoleRepository
{
    public async Task<(IEnumerable<Role>, int)> GetAll(int page, int amount)
    {
        var multi = await connection.QueryMultipleAsync("SP_GetAllRoles", new
        {
            Page = page,
            Amount = amount
        }, commandType: CommandType.StoredProcedure);

        var roles = multi.Read<Role>().ToList();
        var endpoints = multi.Read<PermittedEndpoint>().ToList();
        var totalRoles = await multi.ReadSingleAsync<int>();

        roles = roles
            .Select(r => r with
            {
                PermittedEndpoints =
                endpoints.Where(er => er.RoleId == r.Id).ToList()
            })
            .ToList();

        return (roles, totalRoles);
    }

    public async Task<IEnumerable<RolesCounts>> GetRolesCounts()
    {
        var result = await connection.QueryAsync<RolesCounts>("[dbo].[SP_GetRolesCounts]");

        return result;
    }

    public async Task<bool> Create(Role role, int userId)
    {
        role.RoleCreatorId = userId;
        
        var permittedEndpointsTable = new DataTable();
        permittedEndpointsTable.Columns.Add("EndpointId", typeof(int));
        
        if(role== null)
            throw new UserFriendlyException(ErrorMessages.RoleDataRequired);
        
        if(!role.PermittedEndpoints.Any())
            throw new UserFriendlyException(ErrorMessages.PermittedEndpointsNotFound);
        
        // Add permitted endpoints
        foreach (var endpoint in role.PermittedEndpoints) permittedEndpointsTable.Rows.Add(endpoint.EndpointId);

        var result = await connection.ExecuteAsync("SP_CreateRole", new
        {
            role.Name,
            role.RoleCreatorId,
            PermittedEndpoints = permittedEndpointsTable
        }, commandType: CommandType.StoredProcedure);

        return result > 1;
    }

    public async Task<bool> Edit(Role role)
    {
        var table = new DataTable();
        table.Columns.Add("RoleId", typeof(int));
        table.Columns.Add("EndpointId", typeof(int));
        table.Columns.Add("IsActive", typeof(bool));
        
        if(!role.PermittedEndpoints.Any())
            throw new UserFriendlyException(ErrorMessages.PermittedEndpointsNotFound);

        foreach (var endpoint in role.PermittedEndpoints)
            table.Rows.Add(role.Id, endpoint.EndpointId, endpoint.IsActive);

        var parameters = new DynamicParameters();
        parameters.Add("@RoleId", role.Id, DbType.Int32);
        parameters.Add("@Name", role.Name, DbType.String);
        parameters.Add("@PermittedEndpoints", table.AsTableValuedParameter("dbo.PermittedEndpoints"));

        var result = await connection.ExecuteAsync("SP_EditRole", parameters, commandType: CommandType.StoredProcedure);

        return result > 1;
    }

    public async Task<bool> Delete(int id)
    {
        var result = await connection.ExecuteAsync("SP_DeleteRole",
            new { Id = id }, commandType: CommandType.StoredProcedure);
        return result == 1;
    }
}