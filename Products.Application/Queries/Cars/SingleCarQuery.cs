using MediatR;
using Products.Domain.Entities.Products.Cars;
using Shared.Responses;

namespace Products.Application.Queries.Cars;

public record SingleCarQuery(int Id) : IRequest<BaseResponse<CarModel>>;