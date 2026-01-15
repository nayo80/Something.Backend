using MapsterMapper;
using MediatR;
using Products.Application.Commands.FootballPlayer;
using Products.Domain.Entities.FootballPlayers;
using Products.Infrastructure.Interface;
using Services.ElasticSearch;
using Shared.Guards;
using Shared.Responses;

namespace Products.Application.Handlers.FootballPlayer;

public class CreatePlayerCommandHandler(IGenericRepository<FootballPlayerModel> repository,IMapper mapper,IElasticEngineService elasticServices) : IRequestHandler<CreatePlayerCommand,BaseResponse<int>>
{
    public async Task<BaseResponse<int>> Handle(CreatePlayerCommand request, CancellationToken cancellationToken)
    {
        Guards.NotNull(request.Player, nameof(request.Player),"Player Model cannot be null");
        var mappedCar = mapper.Map<FootballPlayerModel>(request.Player);
        int resultFromServer = await repository.CreateAsync(mappedCar);
        Guards.GreaterThanZero(resultFromServer,"Player Model cannot be created");
        mappedCar.Id = resultFromServer;
        await elasticServices.IndexProductAsync(mappedCar);
        return new BaseResponse<int>
        {
            Result = resultFromServer
        };
    }
}