using MediatR;
using Products.Domain.Entities.FootballPlayers;
using Shared.Responses;

namespace Products.Application.Queries.FootballPlayer;

public record AllPlayerQuery : IRequest<BaseResponse<IEnumerable<FootballPlayerModel>?>>;