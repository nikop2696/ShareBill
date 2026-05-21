using FluentValidation;
using ShareBill.Modules.Users.Domain.Request;

namespace ShareBill.Modules.Users.Validators
{
    public class UserSignUpRequestValidator : AbstractValidator<UserRequests.UserSignUp>
    {
        public UserSignUpRequestValidator()
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
}
