namespace Authorization.Application.DTO;

public class SignInDto
{
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}