using FluentValidation;
using moosik.api.ViewModels.User;

namespace moosik.api.ViewModels.Validators.User;
public class CreateUserValidator : AbstractValidator<CreateUserViewModel>
{
    public CreateUserValidator()
    {
            RuleFor(x => x.Username).Length(6, 100).WithMessage("Username must be between 6 and 100 characters in length")
                .NotEmpty().WithMessage("Username must not be empty");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Please enter a valid email address.")
                .NotEmpty().WithMessage("Email address must not be empty");
            RuleFor(x => x.Password).Length(8, 100).WithMessage("Password must be between 8 and 100 in length")
                .NotEmpty().WithMessage("Password must not be empty");
    }
}
