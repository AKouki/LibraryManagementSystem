using LMS.Core.Domain.Books;

namespace LMS.Core.Repositories
{
    public interface IBookRepository : IRepository<Book>
    {
        //Book GetByISBN(string ISBN);
        //IEnumerable<Book> GetByTitle(string title);
        //IEnumerable<Book> GetByAuthorName(string author);
        IEnumerable<Book> GetAllWithAuthorsSubjects();
        Book GetSingleWithAuthorsSubjects(int id);
    }
}