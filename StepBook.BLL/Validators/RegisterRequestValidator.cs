namespace StepBook.BLL.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterDto>
{
    /// <inheritdoc />
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Username).Username();
        RuleFor(x => x.Email).EmailAddress().Matches(RegexPatterns.Email);
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8).MaximumLength(30);
    }
}