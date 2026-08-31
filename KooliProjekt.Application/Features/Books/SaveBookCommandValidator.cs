using System.Linq;
using FluentValidation;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Features.Books
{
    // Valideerimise klass SaveBookCommand käsu jaoks
    // Võetakse programmi poolt külge automaatselt
    public class SaveBookCommandValidator : AbstractValidator<SaveBookCommand>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveBookCommandValidator(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters");

            RuleFor(x => x.Year)
                .InclusiveBetween(1000, 2100).WithMessage("Year must be between 1000 and 2100");

            // Oma loogikaga valideerimise reegel
            // Siin võib kasutada DbContexti klassi
            RuleFor(x => x.AuthorId)
                .Custom((authorId, context) =>
                {
                    if (!_dbContext.Authors.Any(author => author.Id == authorId))
                    {
                        context.AddFailure(nameof(SaveBookCommand.AuthorId), "Cannot find author with id " + authorId);
                    }
                });
        }
    }
}
