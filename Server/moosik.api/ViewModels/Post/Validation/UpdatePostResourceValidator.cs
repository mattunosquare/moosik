using System.Diagnostics.CodeAnalysis;
using FluentValidation;

namespace moosik.api.ViewModels.Post.Validation;
[ExcludeFromCodeCoverage]
public class UpdatePostResourceValidator : AbstractValidator<UpdatePostResourceViewModel>
{
    public UpdatePostResourceValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Title must not be empty");
        RuleFor(x => x.Value).NotEmpty().WithMessage("Value must not be empty");
    }
}