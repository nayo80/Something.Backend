using MediatR;
using Microsoft.AspNetCore.Mvc;
using Products.Application.Commands.Car;
using Products.Application.Dtos.Cars;
using Shared.Responses;

namespace Products.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class CarController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<BaseResponse<bool>> SignUp([FromBody] RequestCarDto carDto)
    {
        return await mediator.Send(new CreateCarCommand(carDto));
    }
}