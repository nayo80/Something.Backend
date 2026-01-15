using MediatR;
using Products.Domain.Entities.FootballPlayers;
using Shared.Responses;

namespace Products.Application.Queries.FootballPlayer;

public record SinglePlayerQuery(int Id) : IRequest<BaseResponse<FootballPlayerModel>>;