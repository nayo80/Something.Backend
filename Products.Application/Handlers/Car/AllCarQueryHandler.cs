using MediatR;
using Products.Application.Queries.Cars;
using Products.Domain.Entities.Products.Cars;
using Products.Infrastructure.Interfaces.Cars;
using Shared.Guards;
using Shared.Responses;

namespace Products.Application.Handlers.Car;

public class AllCarQueryHandler(ICarRepository repository) : IRequestHandler<AllCarQuery,BaseResponse<IEnumerable<CarModel>?>>
{
    public async Task<BaseResponse<IEnumerable<CarModel>?>> Handle(AllCarQuery request, CancellationToken cancellationToken)
    {
        var car = await repository.ReadAllAsync();
        Guards.NotNull(car, nameof(car),"Cars not found");
        return new BaseResponse<IEnumerable<CarModel>?>()
        {
            Result = car
        };

    }
}
