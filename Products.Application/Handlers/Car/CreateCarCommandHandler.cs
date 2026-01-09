using MapsterMapper;
using MediatR;
using Products.Application.Commands.Car;
using Products.Domain.Entities.Products.Cars;
using Products.Infrastructure.Interfaces.Cars;
using Shared.Guards;
using Shared.Responses;

namespace Products.Application.Handlers.Car;

public class CreateCarCommandHandler(ICarRepository repository,IMapper mapper) : IRequestHandler<CreateCarCommand,BaseResponse<bool>>
{
    public async Task<BaseResponse<bool>> Handle(CreateCarCommand request, CancellationToken cancellationToken)
    {
        Guards.NotNull(request.CarDto, nameof(request.CarDto),"Car Model cannot be null");
        var mappedCar = mapper.Map<CarModel>(request.CarDto);
        bool resultFromServer = await repository.CreateAsync(mappedCar);
        Guards.MustBeTrue(resultFromServer,"Car Model cannot be created");
        return new BaseResponse<bool>
        {
            Result = resultFromServer
        };
    }
}