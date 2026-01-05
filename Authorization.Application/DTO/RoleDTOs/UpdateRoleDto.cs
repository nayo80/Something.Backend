using Shared.DTO;
using System.ComponentModel.DataAnnotations;

namespace Authorization.Application.DTO.RoleDTOs;

public record UpdateRoleDto : BaseDto
{
    [RegularExpression(@"^[a-zA-Z\u00C0-\u024F\u1E00-\u1EFF\u0400-\u04FF\u0300-\u036F]+$", 
        ErrorMessage = "Name can only contain Latin and non-Latin characters (no numbers or special symbols).")]
    public string Name { get; set; } = string.Empty;
    public IEnumerable<PermittedEndpoints> PermittedEndpoints { get; set; } = [];
}