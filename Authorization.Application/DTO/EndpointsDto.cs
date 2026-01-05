using Shared.DTO;

namespace Authorization.Application.DTO;

public record EndpointsDto : BaseDto
{
    public string Name { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
}