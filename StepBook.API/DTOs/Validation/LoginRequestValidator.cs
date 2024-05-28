// using FluentValidation;
//
// namespace StepBook.API.DTOs.Validation;
//
// /// <inheritdoc />
// public class LoginRequestValidator : AbstractValidator<LoginDto>
// {
//     /// <inheritdoc />
//     public LoginRequestValidator()
//     {
//         RuleFor(x => x.Username)
//             .NotEmpty()
//             .WithMessage("Username is required")
//             .EmailAddress()
//             .WithMessage("Username is not valid");
//
//         RuleFor(x => x.Password)
//             .NotEmpty()
//             .WithMessage("Password is required")
//             .MinimumLength(6)
//             .WithMessage("Password must be at least 6 characters long");
//     }
// }