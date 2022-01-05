using FluentValidation;

namespace moosik.api.ViewModels.Validators;

public class CreatePostValidator : AbstractValidator<CreatePostViewModel>
{
    public CreatePostValidator()
    {
        RuleFor(x => x.ResourceTypeId).NotEmpty().NotNull();
    }
}