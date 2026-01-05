namespace Shared.Exceptions;

public class UserFriendlyException(ErrorMessages message) : Exception(message.ToString());

public class ErrorResponse
{
    public string Message { get; set; } = string.Empty;
}