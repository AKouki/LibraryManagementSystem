using LMS.Core.Domain.Books;
using System;
using System.Collections.Generic;
using System.Text;

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
