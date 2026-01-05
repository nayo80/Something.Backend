using Shared.DTO;

namespace Authorization.Application.DTO;

public record EndpointsWithCategoryDto : BaseDto
{
    public string CategoryName { get; set; } = string.Empty;
    public IEnumerable<EndpntsDto> Endpoints { get; set; } = [];
}

public record EndpntsDto
{
    public int EndpointId { get; set; }
    public string Name { get; set; } = string.Empty;
}