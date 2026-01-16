using MediatR;
using Products.Application.Queries.FootballPlayer;
using Products.Domain.Entities.FootballPlayers;
using Products.Infrastructure.Interface;
using Services.Redis.Service;
using Shared.Guards;
using Shared.Responses;

namespace Products.Application.Handlers.FootballPlayer;

public class SinglePlayerQueryHandler(
    IGenericRepository<FootballPlayerModel> repository,
    ICacheService cacheService) 
    : IRequestHandler<SinglePlayerQuery, BaseResponse<FootballPlayerModel>>
{
    public async Task<BaseResponse<FootballPlayerModel>> Handle(SinglePlayerQuery request, CancellationToken cancellationToken)
    {
        Guards.GreaterThanZero(request.Id, nameof(request.Id), "Invalid Player ID.");

        string cacheKey = $"player:{request.Id}";

        var cachedPlayer = await cacheService.GetAsync<FootballPlayerModel>(cacheKey, cancellationToken);
        
        if (cachedPlayer is not null)
        {
            return new BaseResponse<FootballPlayerModel>
            {
                Result = cachedPlayer
            };
        }


        var dbPlayer = await repository.ReadAsync(request.Id);

        if (dbPlayer is not null)
        {
            await cacheService.SetAsync(cacheKey, dbPlayer, cancellationToken: cancellationToken);
        }

        return new BaseResponse<FootballPlayerModel>
        {
            Result = dbPlayer
        };
    }
}