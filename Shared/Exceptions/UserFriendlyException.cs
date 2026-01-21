namespace Shared.Exceptions;

// ორი კლასი ერთ ფაილში რა უბედურებაა
public class UserFriendlyException(ErrorMessages message) : Exception(message.ToString());

public class ErrorResponse
{
    public string Message { get; set; } = string.Empty;
}