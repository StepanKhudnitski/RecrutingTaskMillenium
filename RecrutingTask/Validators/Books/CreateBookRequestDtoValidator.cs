using FluentValidation;
using RecrutingTask.Api.Dtos.Books;

namespace RecrutingTask.Api.Validators.Books
{
    public class CreateBookRequestDtoValidator : AbstractValidator<CreateBookRequestDto>
    {
        public CreateBookRequestDtoValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().MaximumLength(BookRestrictions.MaxNameLength);
            RuleFor(x => x.Type).NotNull().IsInEnum();
        }
    }
}
