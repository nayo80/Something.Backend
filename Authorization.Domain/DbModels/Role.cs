using Shared.Dto;

namespace Authorization.Domain.DbModels;

public record Role : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public int RoleCreatorId { get; set; }
    public string CreatorName { get; set; } = string.Empty;
    public IEnumerable<PermittedEndpoint> PermittedEndpoints { get; init; } = [];
    public DateTime CreatedAt { get; set; }
}

public record PermittedEndpoint
{
    public int RoleId { get; set; }
    public int EndpointId { get; set; }
    public string EndpointName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public record RolesCounts
{
    public int RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public int UsersCount { get; set; }
}