using MediatR;
using Products.Application.Queries.FootballPlayer;
using Products.Domain.Entities.FootballPlayers;
using Products.Infrastructure.Interface;
using Services.ElasticSearch;
using Shared.Guards;
using Shared.Responses;

namespace Products.Application.Handlers.FootballPlayer;

public class AllPlayerQueryHandler(
    IGenericRepository<FootballPlayerModel> repository,
    IElasticEngineService elasticServices)
    : IRequestHandler<AllPlayerQuery, BaseResponse<IEnumerable<FootballPlayerModel>?>>
{
    public async Task<BaseResponse<IEnumerable<FootballPlayerModel>?>> Handle(AllPlayerQuery request,
        CancellationToken cancellationToken)
    {
        var elasticData = await elasticServices.GetAllProductsAsync<FootballPlayerModel>();
        if (elasticData != null)
        {
            return new BaseResponse<IEnumerable<FootballPlayerModel>?>()
            {
                Result = elasticData
            };
        }

        var players = await repository.ReadAllAsync();
        Guards.NotNull(players, nameof(players), "Players not found");
        return new BaseResponse<IEnumerable<FootballPlayerModel>?>()
        {
            Result = players
        };
    }
}