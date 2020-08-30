using LMS.Core.Domain.Issues;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LMS.Data.Configurations
{
    public class IssueConfiguration : IEntityTypeConfiguration<Issue>
    {
        public void Configure(EntityTypeBuilder<Issue> builder)
        {
            builder.ToTable("Issues");
            builder.HasKey(i => i.IssueId);

            builder.HasOne(b => b.Book)
                .WithOne(i => i.Issue)
                .HasForeignKey<Issue>(b => b.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(m => m.Member)
                .WithMany(i => i.Issues)
                .HasForeignKey(m => m.MemberId);
            //.OnDelete(DeleteBehavior.Cascade);
        }
    }
}
