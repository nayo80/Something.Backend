using Shared.Dto;

namespace Authorization.Domain.DbModels;

public record Endpoint : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
}

public record EndpointCategory : BaseEntity
{
    public string Name { get; set; } = string.Empty;
}