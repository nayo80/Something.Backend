using MapsterMapper;
using MediatR;
using Products.Application.Commands.Car;
using Products.Domain.Entities.Products.Cars;
using Products.Infrastructure.Interfaces.Cars;
using Shared.Guards;
using Shared.Responses;

namespace Products.Application.Handlers.Car;

public class UpdateCarCommandHandler(ICarRepository repository,IMapper mapper) : IRequestHandler<UpdateCarCommand,BaseResponse<bool>>
{
    public async Task<BaseResponse<bool>> Handle(UpdateCarCommand request, CancellationToken cancellationToken)
    {
        Guards.NotNull(request.CarDto, nameof(request.CarDto),"Car Model cannot be null");
        var mappedCar = mapper.Map<CarModel>(request.CarDto);
        bool resultFromServer = await repository.UpdateAsync(request.Id,mappedCar);
        Guards.MustBeTrue(resultFromServer,"Car Model cannot be updated");
        return new BaseResponse<bool>
        {
            Result = resultFromServer
        };
    }
}