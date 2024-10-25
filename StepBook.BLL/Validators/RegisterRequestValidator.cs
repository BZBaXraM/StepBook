namespace StepBook.BLL.Validators;

public abstract class RegisterRequestValidator : AbstractValidator<RegisterDto>
{
    /// <inheritdoc />
    protected RegisterRequestValidator()
    {
        RuleFor(x => x.Username).Username();
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8).MaximumLength(30);
    }
}