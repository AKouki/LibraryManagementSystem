using LMS.Core.Domain.Issues;

namespace LMS.Core.Repositories
{
    public interface ITransactionRepository : IRepository<Issue>
    {
        IEnumerable<Issue> GetAllIssuedBooks();
        Issue GetByBookId(int bookId);
    }
}