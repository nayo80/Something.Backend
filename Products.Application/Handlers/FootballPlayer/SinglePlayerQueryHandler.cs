using MediatR;
using Products.Application.Queries.FootballPlayer;
using Products.Domain.Entities.FootballPlayers;
using Products.Infrastructure.Interface;
using Services.ElasticSearch;
using Shared.Guards;
using Shared.Responses;

namespace Products.Application.Handlers.FootballPlayer;

public class SinglePlayerQueryHandler(IGenericRepository<FootballPlayerModel> repository,IElasticEngineService elasticServices) : IRequestHandler<SinglePlayerQuery,BaseResponse<FootballPlayerModel>>
{
    public async Task<BaseResponse<FootballPlayerModel>> Handle(SinglePlayerQuery request, CancellationToken cancellationToken)
    {
        Guards.GreaterThanZero(request.Id, nameof(request.Id.ToString),"player not found.");
        var elasticData = await elasticServices.GetProductAsync<FootballPlayerModel>(request.Id);
        if (elasticData is not null)
        {
            return new BaseResponse<FootballPlayerModel>()
            {
                Result = elasticData
            };
        }
        var car = await repository.ReadAsync(request.Id);
        return new BaseResponse<FootballPlayerModel>()
        {
            Result = car
        };

    }
}
