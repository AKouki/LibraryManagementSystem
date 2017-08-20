using LMS.Core.Domain.Books;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Core.Repositories
{
    public interface IAuthorRepository : IRepository<Author>
    {
        IEnumerable<Author> GetAllWithBooks();
        Author GetSingleWithBooks(int id);
    }
}
