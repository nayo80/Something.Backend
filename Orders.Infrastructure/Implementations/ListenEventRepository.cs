using System.Data;
using Orders.Infrastructure.Interfaces;
using Dapper;
using Shared.Events;

namespace Orders.Infrastructure.Implementations;

public class ListenEventRepository(IDbConnection connection) : IListenEventRepository<CarEventModel>
{
    public async Task InsertOrUpdate(CarEventModel model)
    {
        await connection.ExecuteScalarAsync<int>("dbo.CreateCarReplica", new
        {
            model.CarId,
            model.Brand,
            model.Model,
            model.ReleaseDate,
            model.Price
        }, commandType: CommandType.StoredProcedure);
    }
}