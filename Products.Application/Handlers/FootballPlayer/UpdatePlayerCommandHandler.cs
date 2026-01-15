using MapsterMapper;
using MediatR;
using Products.Application.Commands.FootballPlayer;
using Products.Domain.Entities.FootballPlayers;
using Products.Infrastructure.Interface;
using Services.ElasticSearch;
using Shared.Guards;
using Shared.Responses;

namespace Products.Application.Handlers.FootballPlayer;

public class UpdatePlayerCommandHandler(IGenericRepository<FootballPlayerModel> repository,IMapper mapper,IElasticEngineService elasticServices) : IRequestHandler<UpdatePlayerCommand,BaseResponse<bool>>
{
    public async Task<BaseResponse<bool>> Handle(UpdatePlayerCommand request, CancellationToken cancellationToken)
    {
        Guards.NotNull(request.Player, nameof(request.Player),"Player Model cannot be null");
        var mappedCar = mapper.Map<FootballPlayerModel>(request.Player);
        bool resultFromServer = await repository.UpdateAsync(request.Id,mappedCar);
        Guards.MustBeTrue(resultFromServer,"Player Model cannot be updated");
        await elasticServices.UpdateProductAsync(request.Id, mappedCar);
        return new BaseResponse<bool>
        {
            Result = resultFromServer
        };
    }
}