using FluentValidation;

namespace moosik.api.ViewModels.Validators;

public class CreateThreadValidator : AbstractValidator<CreateThreadViewModel>
{
    public CreateThreadValidator()
    {
        RuleFor(x => x.Title).NotNull().NotEmpty();
        RuleFor(x => x.PostDescription).NotNull().NotEmpty();
        RuleFor(x => x.ThreadTypeId).NotNull().NotEmpty();
        RuleFor(x => x.UserId).NotNull().NotEmpty();
        RuleFor(x => x.PostResourceTitle).NotNull().NotEmpty();
        RuleFor(x => x.PostResourceValue).NotNull().NotEmpty();
        RuleFor(x => x.ResourceTypeId).NotNull().NotEmpty();
    }
}