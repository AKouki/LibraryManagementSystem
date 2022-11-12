#nullable disable
using LMS.Core.Domain.Members;
using LMS.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LMS.Data.Repositories
{
    public class MemberRepository : Repository<Member>, IMemberRepository
    {
        public MemberRepository(LibraryContext context)
            : base(context)
        {
        }

        public Member GetMemberWithIssuedBooks(int id)
        {
            return LibraryContext.Members
                .Include(m => m.Issues)
                    .ThenInclude(b => b.Book)
                .SingleOrDefault(m => m.MemberId == id);
        }

        public LibraryContext LibraryContext
        {
            get { return Context as LibraryContext; }
        }
    }
}
