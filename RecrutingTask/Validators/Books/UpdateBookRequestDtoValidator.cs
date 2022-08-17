using FluentValidation;
using RecrutingTask.Api.Dtos.Books;

namespace RecrutingTask.Api.Validators.Books
{
    public class UpdateBookRequestDtoValidator : AbstractValidator<UpdateBookRequestDto>
    {
        public UpdateBookRequestDtoValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty().NotEqual(Guid.Empty);
            RuleFor(x => x.Name).NotNull().NotEmpty().MaximumLength(BookRestrictions.MaxNameLength);
            RuleFor(x => x.Type).NotNull().IsInEnum();
        }
    }
}
