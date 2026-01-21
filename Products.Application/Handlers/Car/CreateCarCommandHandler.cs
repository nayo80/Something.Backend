using MapsterMapper;
using MassTransit;
using MediatR;
using Products.Application.Commands.Car;
using Products.Domain.Entities.Cars;
using Products.Infrastructure.Interface;
using Services.ElasticSearch;
using Shared.Events;
using Shared.Guards;
using Shared.Responses;

namespace Products.Application.Handlers.Car;

public class CreateCarCommandHandler(
    IGenericRepository<CarModel> repository,
    IMapper mapper,
    IElasticEngineService elasticServices,
    IPublishEndpoint publishEndpoint) : IRequestHandler<CreateCarCommand, BaseResponse<int>>
{
    public async Task<BaseResponse<int>> Handle(CreateCarCommand request, CancellationToken cancellationToken)
    {
        Guards.NotNull(request.CarDto, nameof(request.CarDto), "Car Model cannot be null");
        var mappedCar = mapper.Map<CarModel>(request.CarDto);
        int resultFromServer = await repository.CreateAsync(mappedCar);
        Guards.GreaterThanZero(resultFromServer, "Car Model cannot be created");
        mappedCar.Id = resultFromServer;
        await elasticServices.IndexProductAsync(mappedCar);
        await publishEndpoint.Publish(new CarEventModel
        {
            CarId = resultFromServer,
            Brand = mappedCar.Brand,
            Model = mappedCar.Model,
            Price = mappedCar.Price,
            ReleaseDate = mappedCar.ReleaseDate
        }, context => 
        {
            context.SetRoutingKey("CarCreated");
        }, cancellationToken);

        return new BaseResponse<int>
        {
            Result = resultFromServer
        };
    }
}