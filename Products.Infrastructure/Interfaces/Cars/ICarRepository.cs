using Products.Domain.Entities.Products.Cars;

namespace Products.Infrastructure.Interfaces.Cars;

public interface ICarRepository
{
    Task<bool> CreateAsync(CarModel carModel);
}