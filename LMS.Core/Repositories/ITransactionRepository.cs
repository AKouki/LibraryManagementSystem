using LMS.Core.Domain.Books;
using LMS.Core.Domain.Issues;
using LMS.Core.Domain.Members;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Core.Repositories
{
    public interface ITransactionRepository : IRepository<Issue>
    {
        IEnumerable<Issue> GetAllIssuedBooks();
        Issue GetByBookId(int bookId);
    }
}
