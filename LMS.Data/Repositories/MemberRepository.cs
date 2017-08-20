using LMS.Core.Domain.Members;
using LMS.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
