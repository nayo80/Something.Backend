using Shared.DTO;

namespace Authorization.Domain.Dto;

// domain ში dto - ს რა უნდა
public record UserWithRoleDto : BaseDto
{
    public string Username { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
    public DateTime CreateDate { get; init; } = DateTime.UtcNow;
    public string RoleName { get; init; } = string.Empty;
    public bool IsDeleted { get; init; }
    public string UserGroupName { get; init; } = string.Empty;
    public string DepartmentName { get; init; } = string.Empty;
}