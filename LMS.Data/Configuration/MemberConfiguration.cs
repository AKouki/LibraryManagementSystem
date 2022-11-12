using LMS.Core.Domain.Members;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Data.Configuration
{
    public class MemberConfiguration : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.ToTable("Members");
            builder.HasKey(m => m.MemberId);
            builder.Property(m => m.FirstName).IsRequired();
            builder.Property(m => m.LastName).IsRequired();
            builder.Property(m => m.PhoneNumber).IsRequired();
        }
    }
}
