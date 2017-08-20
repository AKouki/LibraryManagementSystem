using LMS.Core.Domain.Books;
using LMS.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LMS.Data.Repositories
{
    public class AuthorRepository : Repository<Author>, IAuthorRepository
    {
        public AuthorRepository(LibraryContext context)
            : base(context)
        {
        }

        public IEnumerable<Author> GetAllWithBooks()
        {
            return LibraryContext.Authors
                .Include(ba => ba.BookAuthors)
                    .ThenInclude(b => b.Book)
                .ToList();
        }

        public Author GetSingleWithBooks(int id)
        {
            return LibraryContext.Authors
                .Where(a => a.AuthorId == id)
                .Include(ba => ba.BookAuthors)
                    .ThenInclude(b => b.Book)
                .FirstOrDefault();
        }

        public LibraryContext LibraryContext
        {
            get { return Context as LibraryContext; }
        }
    }
}
