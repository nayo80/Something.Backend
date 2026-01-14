using System.Data;
using Dapper;
using Products.Domain.Entities.Products.Cars;
using Products.Infrastructure.Interfaces.Cars;
using Shared.Exceptions;

namespace Products.Infrastructure.Implementations.Cars;

public class CarRepository(IDbConnection connection) : ICarRepository
{
    public async Task<int> CreateAsync(CarModel carModel)
    {
        return await connection.ExecuteScalarAsync<int>("dbo.CreateCar", new
        {
            carModel.Brand,
            carModel.Model,
            carModel.ReleaseDate,
        }, commandType: CommandType.StoredProcedure);
    }

    public async Task<bool> UpdateAsync(int id, CarModel? carModel)
    {
        return await connection.ExecuteScalarAsync<bool>("dbo.UpdateCar", new
        {
            id,
            carModel?.Brand,
            carModel?.Model,
            carModel?.ReleaseDate,
        }, commandType: CommandType.StoredProcedure);
    }

    public async Task<bool> DeleteAsync(int? id)
    {
        return await connection.ExecuteScalarAsync<bool>("dbo.DeleteCar", new
        {
            id
        }, commandType: CommandType.StoredProcedure);
    }

    public async Task<CarModel?> ReadAsync(int id)
    {
        var carModel = await connection.QuerySingleAsync<CarModel?>("dbo.SingleCar", new {id},
            commandType: CommandType.StoredProcedure);
        if (carModel == null) throw new UserFriendlyException(ErrorMessages.CarNotFound);
        return carModel;
    }

    public async Task<IEnumerable<CarModel>?> ReadAllAsync()
    {
        var carModel = await connection.QueryAsync<CarModel>("dbo.AllCar",
            commandType: CommandType.StoredProcedure);
        if (carModel == null) throw new UserFriendlyException(ErrorMessages.CarNotFound);
        return carModel;
    }
}