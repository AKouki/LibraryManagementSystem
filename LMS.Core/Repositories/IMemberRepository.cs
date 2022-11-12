using LMS.Core.Domain.Members;

namespace LMS.Core.Repositories
{
    public interface IMemberRepository : IRepository<Member>
    {
        Member GetMemberWithIssuedBooks(int id);
    }
}