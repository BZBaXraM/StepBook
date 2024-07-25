namespace StepBook.API.DTOs.Validation;

/// <inheritdoc />
public abstract class RegisterRequestValidator : AbstractValidator<RegisterDto>
{
    /// <inheritdoc />
    protected RegisterRequestValidator()
    {
        RuleFor(x => x.Username).Username();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
    }
}