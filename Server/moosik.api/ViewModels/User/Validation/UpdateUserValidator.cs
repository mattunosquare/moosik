using FluentValidation;

namespace moosik.api.ViewModels.User.Validation;

public class UpdateUserValidator : AbstractValidator<UpdateUserViewModel>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.Username).NotEmpty().WithMessage("Username must not be empty");
        RuleFor(x => x.Email).NotEmpty().WithMessage("email must not be empty");
    }
}