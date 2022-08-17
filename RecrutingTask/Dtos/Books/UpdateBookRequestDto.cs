using RecrutingTask.Domain.Models;

namespace RecrutingTask.Api.Dtos.Books
{
    public class UpdateBookRequestDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public BookType Type { get; set; }
    }
}
