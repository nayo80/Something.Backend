using MediatR;
using Products.Application.Queries.Cars;
using Products.Domain.Entities.Products.Cars;
using Products.Infrastructure.Interfaces.Cars;
using Shared.ElasticServices;
using Shared.Guards;
using Shared.Responses;

namespace Products.Application.Handlers.Car;

public class SingleCarQueryHandler(ICarRepository repository,IElasticServices elasticServices) : IRequestHandler<SingleCarQuery,BaseResponse<CarModel>>
{
    public async Task<BaseResponse<CarModel>> Handle(SingleCarQuery request, CancellationToken cancellationToken)
    {
        Guards.GreaterThanZero(request.Id, nameof(request.Id.ToString),"Car not found.");
        var elasticData = await elasticServices.GetProductAsync<CarModel>(request.Id);
        if (elasticData is not null)
        {
            return new BaseResponse<CarModel>()
            {
                Result = elasticData
            };
        }
        var car = await repository.ReadAsync(request.Id);
        return new BaseResponse<CarModel>()
        {
            Result = car
        };

    }
}
