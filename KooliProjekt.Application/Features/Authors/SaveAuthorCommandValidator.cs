using FluentValidation;

namespace KooliProjekt.Application.Features.Authors
{
    // Valideerimise klass SaveAuthorCommand käsu jaoks
    // Võetakse programmi poolt külge automaatselt
    public class SaveAuthorCommandValidator : AbstractValidator<SaveAuthorCommand>
    {
        public SaveAuthorCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("FirstName is required")
                .MaximumLength(50).WithMessage("FirstName cannot exceed 50 characters");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("LastName is required")
                .MaximumLength(50).WithMessage("LastName cannot exceed 50 characters");
        }
    }
}
