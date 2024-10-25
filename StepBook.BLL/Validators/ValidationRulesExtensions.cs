namespace StepBook.BLL.Validators;

/// <summary>
/// Validation rules extensions
/// </summary>
public static class ValidationRulesExtensions
{
    /// <summary>
    /// Add a rule to check if a string is a valid username
    /// </summary>
    /// <param name="rule"></param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, string> Username<T>(this IRuleBuilder<T, string> rule)
        => rule
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(20)
            .Matches("^[a-zA-Z0-9_]*$");

    /// <summary>
    /// Add a rule to check if a string is a valid password
    /// </summary>
    /// <param name="rule"></param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, string> Password<T>(this IRuleBuilder<T, string> rule)
        => rule
            .NotEmpty()
            .MinimumLength(6)
            .Matches("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).{6,}$");

    /// <summary>
    /// Add a rule to check if a string is a valid email
    /// </summary>
    /// <param name="rule"></param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, string> Email<T>(this IRuleBuilder<T, string> rule)
        => rule
            .NotEmpty()
            .EmailAddress();
}