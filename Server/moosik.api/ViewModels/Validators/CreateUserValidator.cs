using FluentValidation;

namespace moosik.api.ViewModels.Validators;
public class CreateUserValidator : AbstractValidator<UserViewModel>
{
    public CreateUserValidator()
    {
            RuleFor(x => x.Username).Length(6, 100).NotNull();
            RuleFor(x => x.Email).EmailAddress().WithMessage("Please enter a valid email address.").NotNull();
    }
}
