namespace Shared.Exceptions;

public class UserFriendlyException(ErrorMessages message) : Exception(message.ToString());

