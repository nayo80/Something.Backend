using MediatR;
using Microsoft.AspNetCore.Mvc;
using Products.Application.Commands.FootballPlayer;
using Products.Application.Dtos.FootballPlayers;
using Products.Application.Queries.FootballPlayer;
using Products.Domain.Entities.FootballPlayers;
using Shared.Responses;

namespace Products.Api.Controllers;
[ApiController]
[Route("api/[controller]/[action]")]
public class FootballPlayerController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<BaseResponse<int>> CreateCar([FromBody] RequestFootballPlayer carDto)
    {
        return await mediator.Send(new CreatePlayerCommand(carDto));
    }
    
    [HttpPut("{id:int}")]
    public async Task<BaseResponse<bool>> UpdateCar( int id,[FromBody] RequestFootballPlayer carDto)
    {
        return await mediator.Send(new UpdatePlayerCommand(id,carDto));
    }
    
    [HttpDelete("{id:int}")]
    public async Task<BaseResponse<bool>> DeleteCar( int id)
    {
        return await mediator.Send(new DeletePlayerCommand(id));
    }
    
    [HttpGet("{id:int}")]
    public async Task<BaseResponse<FootballPlayerModel>> SingleCar( int id)
    {
        return await mediator.Send(new SinglePlayerQuery(id));
    }
    
    [HttpGet]
    public async Task<BaseResponse<IEnumerable<FootballPlayerModel>?>> AllCar()
    {
        return await mediator.Send(new AllPlayerQuery());
    }
}