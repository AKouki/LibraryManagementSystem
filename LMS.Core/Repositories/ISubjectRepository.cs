using LMS.Core.Domain.Books;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Core.Repositories
{
    public interface ISubjectRepository : IRepository<Subject>
    {
        IEnumerable<Subject> GetAllWithBooks();
        Subject GetSingleWithBooks(int id);
    }
}
