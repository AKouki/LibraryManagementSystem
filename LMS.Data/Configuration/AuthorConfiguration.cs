using LMS.Core.Domain.Books;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Data.Configuration
{
    class AuthorConfiguration
    {
        public AuthorConfiguration(EntityTypeBuilder<Author> builder)
        {
            builder.ToTable("Author");
            builder.HasIndex(a => a.Name).IsUnique();
        }
    }
}
