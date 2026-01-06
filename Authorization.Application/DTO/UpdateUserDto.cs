using System.ComponentModel.DataAnnotations;

namespace Authorization.Application.DTO;

public class UpdateUserDto
{
    public int Id { get; init; } 

    [Required(ErrorMessage = "First name is required.")]
    [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
    public string FirstName { get; init; } = string.Empty;

    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
    public string LastName { get; init; } = string.Empty;

    [EmailAddress] public string Email { get; init; } = string.Empty;

    [Required(ErrorMessage = "PhoneNumber is required.")]
    [RegularExpression(@"^(\+9955\d{8}|5\d{8})$", ErrorMessage = "PhoneNumber must be a valid Georgian mobile number.")]
    public string PhoneNumber { get; init; } = string.Empty;

    // [GeneratedRegex(@"^(?=.*[A-Z])(?=.*\d)(?=.*[!@#\$%\^&\*\(\)\-_=\+\[\]\{\};:'"",<>\./?\\|`]).+$")]
    // private static partial Regex PasswordRegex();
}