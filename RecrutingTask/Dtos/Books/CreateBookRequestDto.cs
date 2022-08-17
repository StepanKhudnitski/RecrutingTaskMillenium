using RecrutingTask.Domain.Models;

namespace RecrutingTask.Api.Dtos.Books
{
    public class CreateBookRequestDto
    {
        public string Name { get; set; }
        public BookType Type { get; set; }
    }
}
