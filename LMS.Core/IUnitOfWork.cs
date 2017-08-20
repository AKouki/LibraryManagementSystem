using LMS.Core.Domain.Books;
using LMS.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IBookRepository Books { get; }
        IMemberRepository Members { get; }
        ITransactionRepository Issues { get; }
        IAuthorRepository Authors { get; }
        ISubjectRepository Subjects { get; }
        int Save();
    }
}
