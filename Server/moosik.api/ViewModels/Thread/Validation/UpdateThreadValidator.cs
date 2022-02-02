using FluentValidation;

namespace moosik.api.ViewModels.Thread.Validation;

public class UpdateThreadValidator: AbstractValidator<UpdateThreadViewModel>
{
    public UpdateThreadValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Title must not be empty");
    }
}