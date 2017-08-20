using LMS.Core.Domain.Books;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Data.Configuration
{
    class BookSubjectConfiguration
    {
        public BookSubjectConfiguration(EntityTypeBuilder<BookSubject> builder)
        {
            builder.ToTable("BookSubject");
            builder.HasKey(t => new { t.BookId, t.SubjectId });

            builder.HasOne(bs => bs.Book)
                .WithMany(s => s.BookSubjects)
                .HasForeignKey(bs => bs.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(bs => bs.Subject)
                .WithMany(s => s.BookSubjects)
                .HasForeignKey(bs => bs.SubjectId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
