using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Shared.DTO;

public partial record ResetPasswordDto
{
    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "You must use at least 1 uppercase letter and a number and minimum 1 character.")]
    public string Password { get; init; } = string.Empty;

    [Required(ErrorMessage = "Confirm password is required.")]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public bool IsValidPassword()
    {
        return PasswordRegex().IsMatch(Password);
    }

    [GeneratedRegex(@"^(?=.*[A-Z])(?=.*\d)(?=.*[!@#\$%\^&\*\(\)\-_=\+\[\]\{\};:'"",<>\./?\\|`]).+$")]
    private static partial Regex PasswordRegex();
}