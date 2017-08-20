using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Core.Domain.Books
{
    public class BookSubject
    {
        public int BookId { get; set; }
        public Book Book { get; set; }

        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
    }
}
