using Shared.DTO;

namespace Authorization.Application.DTO.RoleDTOs;

public record PagedRolesResponse
{
    public IEnumerable<ResponseRoleDto> Roles { get; set; } = [];
    public int Page { get; set; }
    public int Amount { get; set; }
    public int TotalAmount { get; set; }
}

public record ResponseRoleDto : BaseDto
{
    //role name
    public string Name { get; set; } = string.Empty;
    public int RoleCreatorId { get; set; }
    public string CreatorName { get; set; } = string.Empty;
    public IEnumerable<ResponsePermittedEndpoints> PermittedEndpoints { get; set; } = [];
    public DateTime CreatedAt { get; set; }
}

public record ResponsePermittedEndpoints
{
    public int EndpointId { get; set; }
    public string EndpointName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public record PermittedEndpoints
{
    public int EndpointId { get; set; }
    public bool IsActive { get; set; }
}

public record RolesCounts
{
    public int RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public int UsersCount { get; set; }
}