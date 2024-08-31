using StepBook.API.Enums;

namespace StepBook.API.Exceptions;

/// <inheritdoc />
public class AuthException : Exception
{
    /// <summary>
    ///     Auth error type
    /// </summary>
    public AuthErrorTypes AuthErrorType { get; set; }

    public AuthException(AuthErrorTypes authErrorType, string message) : base(message)
    {
        AuthErrorType = authErrorType;
    }
}