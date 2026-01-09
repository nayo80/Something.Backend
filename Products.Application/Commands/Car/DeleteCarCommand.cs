using MediatR;
using Products.Application.Dtos.Cars;
using Shared.Responses;

namespace Products.Application.Commands.Car;

public record DeleteCarCommand(int? Id) : IRequest<BaseResponse<bool>>;