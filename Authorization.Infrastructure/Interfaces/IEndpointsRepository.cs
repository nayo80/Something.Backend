using Authorization.Domain.DbModels;

namespace Authorization.Infrastructure.Interfaces;

public interface IEndpointsRepository
{
    Task<IEnumerable<Endpoint>> GetAllAsync();
}