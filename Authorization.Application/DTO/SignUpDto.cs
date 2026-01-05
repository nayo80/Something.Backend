using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Authorization.Application.DTO;

public partial record SignUpDto
{
    [Required(ErrorMessage = "FirstName is required.")]
    [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
    public string FirstName { get; init; } = string.Empty;

    [Required(ErrorMessage = "LastName is required.")]
    [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
    public string LastName { get; init; } = string.Empty;
    [Required]
    [EmailAddress] public string Email { get; init; } = string.Empty;
    [RegularExpression(@"^(\+9955\d{8}|5\d{8})$", ErrorMessage = "PhoneNumber must be a valid Georgian mobile number.")]
    public string PhoneNumber { get; init; } = string.Empty;
    
    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "You must use at least 1 uppercase letter and a number and minimum 1 character.")]
    public string Password { get; init; } = string.Empty;

    [Required(ErrorMessage = "Confirm password is required.")]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty;
    [Required(ErrorMessage = "Role is required")]
    public int RoleId { get; set; }
    public List<int>TagIds { get; init; } = new List<int>();

    public bool IsValidPassword()
    {
        return PasswordRegex().IsMatch(Password);
    }

    [GeneratedRegex(@"^(?=.*[A-Z])(?=.*\d)(?=.*[!@#\$%\^&\*\(\)\-_=\+\[\]\{\};:'"",<>\./?\\|`]).+$")]
    private static partial Regex PasswordRegex();
}