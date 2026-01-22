namespace Orders.Infrastructure.Interfaces;

public interface IListenEventRepository<T> where T : class
{
    Task InsertOrUpdate(T model);
}