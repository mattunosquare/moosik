using FluentValidation;

namespace moosik.api.ViewModels.Validators;

public class UpdateUserValidator : AbstractValidator<UserViewModel>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.Id).NotEqual(0);
    }
}