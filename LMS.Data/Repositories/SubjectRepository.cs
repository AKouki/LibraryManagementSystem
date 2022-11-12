#nullable disable
using LMS.Core.Domain.Books;
using LMS.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LMS.Data.Repositories
{
    public class SubjectRepository : Repository<Subject>, ISubjectRepository
    {
        public SubjectRepository(LibraryContext context)
            : base(context)
        {
        }

        public IEnumerable<Subject> GetAllWithBooks()
        {
            return LibraryContext.Subjects
                .Include(bs => bs.BookSubjects)
                .ToList();
        }

        public Subject GetSingleWithBooks(int id)
        {
            return LibraryContext.Subjects
                .Where(a => a.SubjectId == id)
                .Include(bs => bs.BookSubjects)
                    .ThenInclude(b => b.Book)
                .FirstOrDefault();
        }

        public LibraryContext LibraryContext
        {
            get { return Context as LibraryContext; }
        }
    }
}
