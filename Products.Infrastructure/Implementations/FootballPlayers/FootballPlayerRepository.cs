using System.Data;
using Dapper;
using Products.Domain.Entities.FootballPlayers;
using Products.Infrastructure.Interface;
using Shared.Exceptions;

namespace Products.Infrastructure.Implementations.FootballPlayers;

public class FootballPlayerRepository(IDbConnection connection) : IGenericRepository<FootballPlayerModel>
{
    public async Task<int> CreateAsync(FootballPlayerModel carModel)
    {
        return await connection.ExecuteScalarAsync<int>("dbo.CreatePlayer", new
        {
            carModel.FirstName,
            carModel.LastName,
            carModel.FootballClub,
        }, commandType: CommandType.StoredProcedure);
    }

    public async  Task<bool> UpdateAsync(int id, FootballPlayerModel? carModel)
    {
        return await connection.ExecuteScalarAsync<bool>("dbo.UpdatePlayer", new
        {
            id,
            carModel?.FirstName,
            carModel?.LastName,
            carModel?.FootballClub,
        }, commandType: CommandType.StoredProcedure);
    }

    public async Task<bool> DeleteAsync(int? id)
    {
        return await connection.ExecuteScalarAsync<bool>("dbo.DeletePlayer", new
        {
            id
        }, commandType: CommandType.StoredProcedure);
    }

    public async Task<FootballPlayerModel?> ReadAsync(int id)
    {
        var playerModel = await connection.QuerySingleOrDefaultAsync<FootballPlayerModel?>("dbo.SinglePlayer", new {id},
            commandType: CommandType.StoredProcedure);
        if (playerModel == null) throw new UserFriendlyException(ErrorMessages.PlayerNotFound);
        return playerModel;
    }

    public async Task<IEnumerable<FootballPlayerModel>?> ReadAllAsync()
    {
        var playerModel = await connection.QueryAsync<FootballPlayerModel>("dbo.AllPlayers",
            commandType: CommandType.StoredProcedure);
        if (playerModel == null) throw new UserFriendlyException(ErrorMessages.PlayerNotFound);
        return playerModel;
    }
}