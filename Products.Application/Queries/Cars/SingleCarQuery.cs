using MediatR;
using Products.Domain.Entities.Cars;
using Shared.Responses;

namespace Products.Application.Queries.Cars;

public record SingleCarQuery(int Id) : IRequest<BaseResponse<CarModel>>;