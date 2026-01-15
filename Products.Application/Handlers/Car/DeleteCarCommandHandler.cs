using MediatR;
using Products.Application.Commands.Car;
using Products.Domain.Entities.Cars;
using Products.Infrastructure.Interface;
using Services.ElasticSearch;
using Shared.Guards;
using Shared.Responses;

namespace Products.Application.Handlers.Car;

public class DeleteCarCommandHandler(IGenericRepository<CarModel> repository,IElasticEngineService elasticServices) : IRequestHandler<DeleteCarCommand,BaseResponse<bool>>
{
    public async Task<BaseResponse<bool>> Handle(DeleteCarCommand request, CancellationToken cancellationToken)
    {
        Guards.NotNull(request.Id, nameof(request),"Can't pick a car");
        bool resultFromServer = await repository.DeleteAsync(request.Id);
        Guards.MustBeTrue(resultFromServer,"Car Model cannot be Deleted");
        await elasticServices.DeleteProductAsync(request.Id);
        return new BaseResponse<bool>
        {
            Result = resultFromServer
        };
    }
}