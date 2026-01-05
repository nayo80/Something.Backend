using Authorization.Domain.DbModels;
using Authorization.Infrastructure.Interfaces;
using Dapper;
using System.Data;

namespace Authorization.Infrastructure.Implementation;

public class EndpointsRepository(IDbConnection connection) : IEndpointsRepository
{
    public async Task<IEnumerable<Endpoint>> GetAllAsync()
    {
        var grid = await connection.QueryMultipleAsync("[dbo].[SP_GetEndpoints]");

        var categories = (await grid.ReadAsync<EndpointCategory>()).ToList();
        var endpoints = await grid.ReadAsync<Endpoint>();

        foreach (var endpoint in endpoints)
        {
            endpoint.CategoryName = categories.First(c => c.Id == endpoint.CategoryId).Name;
        }

        return endpoints;
    }
}