namespace Services.ElasticSearch;

public interface IElasticEngineService
{
    Task<T?> GetProductAsync<T>(int id) where T : class;
    Task IndexProductAsync<T>(T product);
    Task UpdateProductAsync<T>(int id, T updatedProduct) where T : class;
    Task DeleteProductAsync(int id);
}