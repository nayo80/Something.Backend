namespace Shared.Guards;

using System.Diagnostics.CodeAnalysis;

public static class Guards
{
    public static void NotNull([NotNull] object? arg, string argName, string? message = null)
    {
        if (arg is null)
            throw new ArgumentNullException(argName, message ?? $"Argument '{argName}' cannot be null.");
    }
    public static void GreaterThanZero(int value, string paramName, string? message = null)
    {
        if (value <= 0)
            throw new ArgumentOutOfRangeException(paramName, message ?? $"{paramName} must be greater than zero.");
    }
    public static void MustBeTrue(bool value,string? message = null)
    {
        if (!value)
            throw new Exception(message ?? $"If the email is registered, a reset link has been sent.");
    }
}
