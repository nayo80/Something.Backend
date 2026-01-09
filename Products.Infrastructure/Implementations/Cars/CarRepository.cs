using System.Data;
using Dapper;
using Products.Domain.Entities.Products.Cars;
using Products.Infrastructure.Interfaces.Cars;

namespace Products.Infrastructure.Implementations.Cars;

public class CarRepository(IDbConnection connection) : ICarRepository
{
    public async Task<bool> CreateAsync(CarModel carModel)
    {
        return await connection.ExecuteScalarAsync<bool>("dbo.CreateCar", new
        {
            carModel.Brand,
            carModel.Model,
            carModel.ReleaseDate,
        }, commandType: CommandType.StoredProcedure);
    }
}