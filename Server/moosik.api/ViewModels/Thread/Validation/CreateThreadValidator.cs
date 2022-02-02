using FluentValidation;

namespace moosik.api.ViewModels.Thread.Validation;

public class CreateThreadValidator : AbstractValidator<CreateThreadViewModel>
{
    public CreateThreadValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Title must not be empty");
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId must not be empty");
        RuleFor(x => x.ThreadTypeId).NotEmpty().WithMessage("ThreadTypeId must not be empty");
        RuleFor(x => x.Post).NotEmpty().WithMessage("Post must not be null");
    }
}