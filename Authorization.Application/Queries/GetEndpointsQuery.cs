using Authorization.Application.DTO;
using MediatR;

namespace Authorization.Application.Queries;

public class GetEndpointsQuery : IRequest<IEnumerable<EndpointsWithCategoryDto>>;