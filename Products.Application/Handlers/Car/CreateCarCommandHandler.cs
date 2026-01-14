using MapsterMapper;
using MediatR;
using Products.Application.Commands.Car;
using Products.Domain.Entities.Products.Cars;
using Products.Infrastructure.Interfaces.Cars;
using Products.Infrastructure.Interfaces.Elastic;
using Shared.Guards;
using Shared.Responses;

namespace Products.Application.Handlers.Car;

public class CreateCarCommandHandler(ICarRepository repository,IMapper mapper,IElasticSearchService productSearchService) : IRequestHandler<CreateCarCommand,BaseResponse<int>>
{
    public async Task<BaseResponse<int>> Handle(CreateCarCommand request, CancellationToken cancellationToken)
    {
        Guards.NotNull(request.CarDto, nameof(request.CarDto),"Car Model cannot be null");
        var mappedCar = mapper.Map<CarModel>(request.CarDto);
        int resultFromServer = await repository.CreateAsync(mappedCar);
        Guards.GreaterThanZero(resultFromServer,"Car Model cannot be created");
        mappedCar.Id = resultFromServer;
        await productSearchService.IndexProductAsync(mappedCar);
        return new BaseResponse<int>
        {
            Result = resultFromServer
        };
    }
}