using MediatR;
using Products.Domain.Entities.Products.Cars;
using Shared.Responses;

namespace Products.Application.Queries.Cars;

public record AllCarQuery : IRequest<BaseResponse<IEnumerable<CarModel>?>>;