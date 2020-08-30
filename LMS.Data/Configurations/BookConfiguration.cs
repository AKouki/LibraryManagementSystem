using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LMS.Core.Domain.Books;

namespace LMS.Data.Configurations
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("Books");
            builder.HasKey(b => b.BookId);
            builder.HasIndex(b => b.ISBN).IsUnique();
            builder.HasOne(i => i.Issue).WithOne(b => b.Book).HasForeignKey<Book>(b => b.BookId);
            builder.Property(b => b.Title).IsRequired();
            builder.Property(b => b.Publisher).IsRequired();
            builder.Property(b => b.ISBN).IsRequired();
            builder.Property(b => b.MaxIssueDays).HasDefaultValue(10);
        }
    }
}
