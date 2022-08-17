using Microsoft.AspNetCore.Mvc;
using RecrutingTask.Api.Dtos.Books;
using RecrutingTask.Domain.Models;
using RecrutingTask.Domain.Repos;
using System.ComponentModel.DataAnnotations;

namespace RecrutingTask.Api.Controllers.Books
{

    [Route("api/[controller]")]
    [ApiController]
    [FormatFilter]
    public class BookController : ControllerBase
    {
        private readonly ILogger<BookController> _logger;
        private readonly IBookRepo _repo;

        public BookController(IBookRepo bookRepo, ILogger<BookController> logger)
        {
            _logger = logger;
            _repo = bookRepo;
        }

        /// <summary>
        /// Get book by id.
        /// </summary>
        /// <param name="id"> Id of the book to be retrieved. </param>
        /// <param name="cancellationToken"> Token utilized for request cancellation.</param>
        /// <returns>Retrieved book.</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     Get /GetAsync/3fa85f64-5717-4562-b3fc-2c963f66afa6
        ///     {
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Returns the book</response>
        /// <response code="404">If the item with such id is not found</response>
        [HttpGet("{id}/{format?}")]
        [FormatFilter]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Book), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAsync([FromRoute, Required] Guid id, CancellationToken cancellationToken)
        {
            var retrievedBook = await _repo.GetFirstOrDefaultAsync(x => x.Id == id);

            if (retrievedBook == null)
            {
                _logger.LogWarning($"Book with id {id} not found.");
                return NotFound();
            }

            return Ok(retrievedBook);
        }

        /// <summary>
        /// Create new book.
        /// </summary>
        /// <param name="dto">Model of the book to be created.</param>
        /// <param name="cancellationToken">Token utilized for request cancellation.</param>
        /// <returns>Id of the created Book in Guid format.</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     Post /CreateAsync
        ///     {
        ///         "name": "Robinsons from Polesie",
        ///         "type": 0
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Returns the id of the newly created Book</response>
        /// <response code="400">If the item is null</response>
        [HttpPost("{format?}")]
        [FormatFilter]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Guid>> CreateAsync([FromBody] CreateBookRequestDto dto, CancellationToken cancellationToken)
        {
            var book = new Book
            {
                Type = dto.Type,
                Name = dto.Name
            };

            var createdBookId = await _repo.CreateAsync(book, cancellationToken);

            _logger.LogInformation("Book created successfully.");
            return Ok(createdBookId);
        }

        /// <summary>
        /// Update existing book.
        /// </summary>
        /// <param name="dto">Model of the book to be updated.</param>
        /// <param name="cancellationToken">Token utilized for request cancellation</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     Put /UpdateAsync
        ///     {
        ///         "id"  : "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///         "name": "Robinsons from Polesie",
        ///         "type": 0
        ///     }
        ///
        /// </remarks>
        /// <response code="200"></response>
        /// <response code="400">If the item is null</response>
        /// <response code="404">If the item with such id is not found</response>
        [HttpPut("{format?}")]
        [FormatFilter]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateBookRequestDto dto, CancellationToken cancellationToken)
        {
            var findExisting = await _repo.GetFirstOrDefaultAsync(book => book.Id == dto.Id, cancellationToken);

            if (findExisting == null)
            {
                _logger.LogWarning($"Book with id {dto.Id} not found.");
                return NotFound();
            }

            findExisting.Name = dto.Name;
            findExisting.Type = dto.Type;

            await _repo.UpdateAsync(findExisting, cancellationToken);

            return Ok();
        }

        /// <summary>
        /// Deletes car by Id.
        /// </summary>
        /// <param name="id">Id of the book to be deleted.</param>
        /// <param name="cancellationToken">Token utilized for request cancellation</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     Delete /DeleteAsync/3fa85f64-5717-4562-b3fc-2c963f66afa6
        ///     {
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Item was deleted successfully</response>
        /// <response code="400">If the id is null</response>
        /// <response code="404">If the item with such id is not found</response>
        [HttpDelete("{id}/{format?}")]
        [FormatFilter]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync([FromRoute, Required] Guid id, CancellationToken cancellationToken)
        {
            var toBeDeletedBook = await _repo.GetFirstOrDefaultAsync(x => x.Id == id);

            if (toBeDeletedBook == null)
            {
                _logger.LogWarning($"Book with id {id} not found.");
                return NotFound();
            }

            await _repo.DeleteAsync(id, cancellationToken);
            return Ok();
        }
    }
}
