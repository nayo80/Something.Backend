using MediatR;
using Products.Application.Dtos.FootballPlayers;
using Shared.Responses;

namespace Products.Application.Commands.FootballPlayer;

public record UpdatePlayerCommand(int Id,RequestFootballPlayer? Player) : IRequest<BaseResponse<bool>>;
