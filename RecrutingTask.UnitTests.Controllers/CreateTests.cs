using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RecrutingTask.Api.Controllers.Books;
using RecrutingTask.Api.Dtos.Books;
using RecrutingTask.Domain.Models;
using RecrutingTask.Domain.Repos;

namespace RecrutingTask.UnitTests.Controllers
{
    public class CreateTests
    {
        [Fact(DisplayName = "CreateTests TestCreatedSuccessufully")]
        public async Task TestCreatedSuccessufully()
        {
            // Arrange
            var testBook = GenerateTestBook();
            var createdId = Guid.NewGuid();

            var mockRepo = new Mock<IBookRepo>();
            mockRepo.Setup(repo => repo.CreateAsync(It.Is<Book>(b =>
            b.Name == testBook.Name
            && b.Type == testBook.Type), It.IsAny<CancellationToken>())).ReturnsAsync(createdId);

            var loggerMock = new Mock<ILogger<BookController>>();
            var controller = new BookController(mockRepo.Object, loggerMock.Object);

            var dto = new CreateBookRequestDto() { Name = testBook.Name, Type = testBook.Type };
            // Act
            ActionResult<Guid> result = await controller.CreateAsync(dto, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<ActionResult<Guid>>(result);
            var okValue = Assert.IsType<Guid>((okResult.Result as ObjectResult).Value);
            Assert.Equal(okValue, createdId);
        }

        public static Book GenerateTestBook()
        {
            return new Book()
            {
                Name = "Test Book",
                Type = BookType.Horror
            };
        }
    }
}