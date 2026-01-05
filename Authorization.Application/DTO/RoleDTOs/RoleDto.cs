using Shared.DTO;

namespace Authorization.Application.DTO.RoleDTOs;

public record RoleDto : BaseDto
{
    public string Name { get; set; } = string.Empty;
    public IEnumerable<PermittedEndpoints> PermittedEndpoints { get; set; } = [];
    public DateTime CreatedAt { get; set; }
}