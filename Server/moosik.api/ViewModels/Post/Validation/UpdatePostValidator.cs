using System.Diagnostics.CodeAnalysis;
using FluentValidation;

namespace moosik.api.ViewModels.Validators;
[ExcludeFromCodeCoverage]
public class UpdatePostValidator : AbstractValidator<UpdatePostViewModel>
{
    public UpdatePostValidator()
    {
        RuleFor(x => x.Description).NotEmpty().WithMessage("Description must not be empty");
    }
}