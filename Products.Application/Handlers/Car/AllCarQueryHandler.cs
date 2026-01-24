using MediatR;
using Products.Application.Queries.Cars;
using Products.Domain.Entities.Cars;
using Products.Infrastructure.Interface;
using Services.ElasticSearch;
using Shared.Guards;
using Shared.Responses;

namespace Products.Application.Handlers.Car;

public class AllCarQueryHandler(IGenericRepository<CarModel> repository,IElasticEngineService elasticServices) : IRequestHandler<AllCarQuery,BaseResponse<IEnumerable<CarModel>?>>
{
    public async Task<BaseResponse<IEnumerable<CarModel>?>> Handle(AllCarQuery request, CancellationToken cancellationToken)
    {
        var elasticData = await elasticServices.GetAllProductsAsync<CarModel>();
        if (elasticData != null)
        {
            return new BaseResponse<IEnumerable<CarModel>?>()
            {
                Result = elasticData
            };
        }
        var car = await repository.ReadAllAsync();
        Guards.NotNull(car, nameof(car),"Cars not found");
        return new BaseResponse<IEnumerable<CarModel>?>()
        {
            Result = car
        };

    }
}
