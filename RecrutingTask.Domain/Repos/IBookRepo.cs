using RecrutingTask.Domain.Models;
using System.Linq.Expressions;

namespace RecrutingTask.Domain.Repos
{
    public interface IBookRepo
    {
        Task<Guid> CreateAsync(Book book, CancellationToken cancellationToken = default);
        Task<Book> GetFirstOrDefaultAsync(Expression<Func<Book, bool>> filter, CancellationToken cancellationToken = default);
        Task<Book[]> GetWhereAsync(Expression<Func<Book, bool>> filter, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task UpdateAsync(Book book, CancellationToken cancellationToken = default);
    }
}
