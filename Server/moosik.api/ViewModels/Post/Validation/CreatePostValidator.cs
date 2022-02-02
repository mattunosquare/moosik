using FluentValidation;

namespace moosik.api.ViewModels.Validators;

public class CreatePostValidator : AbstractValidator<CreatePostViewModel>
{
    public CreatePostValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User Id must not be empty");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Description must not be empty");
    }
}