using Microsoft.EntityFrameworkCore;
using RecrutingTask.Domain.Models;
using RecrutingTask.Domain.Repos;
using System.Linq.Expressions;

namespace RecrutingTask.Infrastructure.Repos
{
    public class BookRepo : IBookRepo
    {
        private readonly LibraryContext _context;

        public BookRepo(LibraryContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateAsync(Book book, CancellationToken cancellationToken = default)
        {
            await _context.Books.AddAsync(book, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return book.Id;
        }

        public async Task<Book> GetFirstOrDefaultAsync(Expression<Func<Book, bool>> filter, CancellationToken cancellationToken = default)
        {
            var query = GetQuery(filter);
            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Book[]> GetWhereAsync(Expression<Func<Book, bool>> filter, CancellationToken cancellationToken = default)
        {
            var query = GetQuery(filter);
            return await query.ToArrayAsync(cancellationToken);
        }

        private IQueryable<Book> GetQuery(Expression<Func<Book, bool>> filter)
        {
            IQueryable<Book> query = _context.Books;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query;
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var book = await _context.Books.FirstOrDefaultAsync(x => x.Id == id);
            _context.Books.Remove(book);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Book book, CancellationToken cancellationToken = default)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
