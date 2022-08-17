using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecrutingTask.Domain.Models;

namespace RecrutingTask.Infrastructure.Repos
{
    public class BookEntityTypeConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("Books", "books");

            builder.HasKey(c => c.Id);

        }
    }
}
