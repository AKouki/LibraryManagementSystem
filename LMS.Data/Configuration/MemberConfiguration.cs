using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LMS.Core.Domain.Members;

namespace LMS.Data.Configuration
{
    class MemberConfiguration
    {
        public MemberConfiguration(EntityTypeBuilder<Member> builder)
        {
            builder.ToTable("Member");
            builder.HasKey(m => m.MemberId);
            builder.Property(m => m.FirstName).IsRequired();
            builder.Property(m => m.LastName).IsRequired();
            builder.Property(m => m.PhoneNumber).IsRequired();
        }
    }
}
