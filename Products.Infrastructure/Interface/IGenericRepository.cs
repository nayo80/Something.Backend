using Products.Domain.Entities.Products.Cars;

namespace Products.Infrastructure.Interfaces.Cars;

public interface IGenericRepository<T>
{
    Task<int> CreateAsync(T carModel);
    Task<bool> UpdateAsync(int id,T? carModel);
    Task<bool> DeleteAsync(int? id);
    Task<CarModel?> ReadAsync(int id);
    Task<IEnumerable<T>?> ReadAllAsync();
}