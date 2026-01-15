namespace Products.Infrastructure.Interface;

public interface IGenericRepository<T>
{
    Task<int> CreateAsync(T carModel);
    Task<bool> UpdateAsync(int id,T? carModel);
    Task<bool> DeleteAsync(int? id);
    Task<T?> ReadAsync(int id);
    Task<IEnumerable<T>?> ReadAllAsync();
}