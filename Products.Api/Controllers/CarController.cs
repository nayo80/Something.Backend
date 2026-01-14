using MediatR;
using Microsoft.AspNetCore.Mvc;
using Products.Application.Commands.Car;
using Products.Application.Dtos.Cars;
using Products.Application.Queries.Cars;
using Products.Domain.Entities.Products.Cars;
using Shared.Responses;

namespace Products.Api.Controllers;
[ApiController]
[Route("api/[controller]/[action]")]
public class CarController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<BaseResponse<int>> CreateCar([FromBody] RequestCarDto carDto)
    {
        return await mediator.Send(new CreateCarCommand(carDto));
    }
    
    [HttpPut("{id:int}")]
    public async Task<BaseResponse<bool>> UpdateCar( int id,[FromBody] RequestCarDto carDto)
    {
        return await mediator.Send(new UpdateCarCommand(id,carDto));
    }
    
    [HttpDelete("{id:int}")]
    public async Task<BaseResponse<bool>> DeleteCar( int id)
    {
        return await mediator.Send(new DeleteCarCommand(id));
    }
    
    [HttpGet("{id:int}")]
    public async Task<BaseResponse<CarModel>> SingleCar( int id)
    {
        return await mediator.Send(new SingleCarQuery(id));
    }
    
    [HttpGet]
    public async Task<BaseResponse<IEnumerable<CarModel>?>> AllCar()
    {
        return await mediator.Send(new AllCarQuery());
    }
}