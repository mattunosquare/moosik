using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using moosik.api.ViewModels.Post;

namespace moosik.api.ViewModels.Validators;
[ExcludeFromCodeCoverage]
public class CreatePostResourceValidator : AbstractValidator<CreatePostResourceViewModel>
{
    public CreatePostResourceValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Title must not be empty");
        RuleFor(x => x.Value).NotEmpty().WithMessage("Value must not be empty");
        RuleFor(x => x.TypeId).NotEmpty().WithMessage("TypeId must not be empty");
    }
}