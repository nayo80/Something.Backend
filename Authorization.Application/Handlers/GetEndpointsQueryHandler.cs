using Authorization.Application.DTO;
using Authorization.Application.Queries;
using Authorization.Infrastructure.Interfaces;
using MediatR;

namespace Authorization.Application.Handlers;

public class GetEndpointsQueryHandler(IEndpointsRepository repository) 
    : IRequestHandler<GetEndpointsQuery, IEnumerable<EndpointsWithCategoryDto>>
{
    public async Task<IEnumerable<EndpointsWithCategoryDto>> Handle(GetEndpointsQuery request, CancellationToken cancellationToken)
    {
        var result = await repository.GetAllAsync();
        
        var groupedEndpoints = result
            .GroupBy(x => new { x.CategoryId, x.CategoryName })
            .Select(g => new EndpointsWithCategoryDto
            {
                Id = g.Key.CategoryId,
                CategoryName = g.Key.CategoryName,
                Endpoints = g.Select(endpoint => new EndpntsDto
                {
                    EndpointId = endpoint.Id,
                    Name = endpoint.Name
                }).ToList()
            })
            .ToList();

        return groupedEndpoints;
    }
}