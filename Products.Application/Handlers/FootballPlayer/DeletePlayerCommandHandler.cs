using MediatR;
using Products.Application.Commands.Car;
using Products.Application.Commands.FootballPlayer;
using Products.Domain.Entities.FootballPlayers;
using Products.Infrastructure.Interface;
using Services.ElasticSearch;
using Shared.Guards;
using Shared.Responses;

namespace Products.Application.Handlers.FootballPlayer;

public class DeletePlayerCommandHandler(IGenericRepository<FootballPlayerModel> repository,IElasticEngineService elasticServices) : IRequestHandler<DeletePlayerCommand,BaseResponse<bool>>
{
    public async Task<BaseResponse<bool>> Handle(DeletePlayerCommand request, CancellationToken cancellationToken)
    {
        Guards.NotNull(request.Id, nameof(request),"Can't pick a player");
        bool resultFromServer = await repository.DeleteAsync(request.Id);
        Guards.MustBeTrue(resultFromServer,"player  cannot be Deleted");
        await elasticServices.DeleteProductAsync(request.Id);
        return new BaseResponse<bool>
        {
            Result = resultFromServer
        };
    }
}