using FluentValidation;

namespace StepBook.API.DTOs.Validation;

/// <inheritdoc />
public class RegisterRequestValidator : AbstractValidator<RegisterDto>
{
    /// <inheritdoc />
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Username).Username();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
    }
}