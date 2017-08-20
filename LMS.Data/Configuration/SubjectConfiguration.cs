using LMS.Core.Domain.Books;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Data.Configuration
{
    public class SubjectConfiguration
    {
        public SubjectConfiguration(EntityTypeBuilder<Subject> builder)
        {
            builder.ToTable("Subject");
            builder.HasIndex(s => s.Name).IsUnique();
        }
    }
}
