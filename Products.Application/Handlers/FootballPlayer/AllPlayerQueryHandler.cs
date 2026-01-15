using MediatR;
using Products.Application.Queries.FootballPlayer;
using Products.Domain.Entities.FootballPlayers;
using Products.Infrastructure.Interface;
using Shared.Guards;
using Shared.Responses;

namespace Products.Application.Handlers.FootballPlayer;

public class AllPlayerQueryHandler(IGenericRepository<FootballPlayerModel> repository) : IRequestHandler<AllPlayerQuery,BaseResponse<IEnumerable<FootballPlayerModel>?>>
{
    public async Task<BaseResponse<IEnumerable<FootballPlayerModel>?>> Handle(AllPlayerQuery request, CancellationToken cancellationToken)
    {
        var car = await repository.ReadAllAsync();
        Guards.NotNull(car, nameof(car),"Players not found");
        return new BaseResponse<IEnumerable<FootballPlayerModel>?>()
        {
            Result = car
        };

    }
}
