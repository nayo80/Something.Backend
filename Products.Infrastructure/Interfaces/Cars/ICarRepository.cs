using Products.Domain.Entities.Products.Cars;

namespace Products.Infrastructure.Interfaces.Cars;

public interface ICarRepository
{
    Task<bool> CreateAsync(CarModel carModel);
    Task<bool> UpdateAsync(int id,CarModel? carModel);
    Task<bool> DeleteAsync(int? id);
    Task<CarModel?> ReadAsync(int id);
    Task<IEnumerable<CarModel>?> ReadAllAsync();
}