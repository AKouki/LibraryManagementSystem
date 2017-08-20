using LMS.Core.Domain.Members;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Core.Repositories
{
    public interface IMemberRepository : IRepository<Member>
    {
        Member GetMemberWithIssuedBooks(int id);
    }
}
