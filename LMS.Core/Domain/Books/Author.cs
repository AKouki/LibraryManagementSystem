using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Core.Domain.Books
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string Name { get; set; }

        public List<BookAuthor> BookAuthors { get; set; }
    }
}
    