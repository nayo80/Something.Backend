using MediatR;
using Products.Application.Dtos.FootballPlayers;
using Shared.Responses;

namespace Products.Application.Commands.FootballPlayer;

public record CreatePlayerCommand(RequestFootballPlayer Player) : IRequest<BaseResponse<int>>;
