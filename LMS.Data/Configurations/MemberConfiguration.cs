using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LMS.Core.Domain.Members;

namespace LMS.Data.Configurations
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
