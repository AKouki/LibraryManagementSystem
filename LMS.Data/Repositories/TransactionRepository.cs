using LMS.Core.Domain.Issues;
using LMS.Core.Repositories;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace LMS.Data.Repositories
{
    public class TransactionRepository : Repository<Issue>, ITransactionRepository
    {
        public TransactionRepository(LibraryContext context) 
            : base(context)
        {
        }

        public IEnumerable<Issue> GetAllIssuedBooks()
        {
            return LibraryContext.Issues
                .Include(b => b.Book)
                .Include(m => m.Member)
                .ToList();
        }

        public Issue GetByBookId(int bookId)
        {
            return LibraryContext.Issues
                .Where(i => i.BookId == bookId)
                .Include(b => b.Book)
                .Include(m => m.Member)
                .FirstOrDefault();
        }

        public LibraryContext LibraryContext
        {
            get { return Context as LibraryContext; }
        }
    }
}
