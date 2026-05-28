using FluentValidation;
using ShareBill.Modules.Users.Domain.Request;

namespace ShareBill.Modules.Users.Validators
{
    /// <summary>
    /// Validator for user sign-up requests.
    /// Ensures that the email, password, and username meet the required criteria.
    /// </summary>
    public class UserRequestValidator : AbstractValidator<UserRequests.UserSignUp>
    {
        public UserRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
            
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain at least one digit.")
                .Matches(@"^[A-Za-z0-9!@#$%^&*(),.?"":{}|<>_\-]+$")
                .WithMessage("Password contains invalid characters. Special allowed characters are: ! @ # $ % ^ & * ( ) , . ? : { } | < > _ -.");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .MinimumLength(7).WithMessage("Username must be at least 7 characters long.")
                .MaximumLength(20).WithMessage("Username must be no more than 20 characters long.")
                .Matches("^[a-zA-Z0-9_]+$").WithMessage("Username can only contain letters, numbers, and underscores.");
        }
    }

    /// <summary>
    /// Provides validation rules for login requests, ensuring that required fields are present and correctly formatted.
    /// </summary>
    /// <remarks>This validator checks that the email is not empty and is in a valid email format, and that
    /// the password is not empty. Use this class to validate user input before processing login attempts.</remarks>
    public class LoginRequestValidator : AbstractValidator<LoginRequest.Login>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
            
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");
        }
    }
}
