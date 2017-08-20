using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Core.Domain.Books
{
    public class Subject
    {
        public int SubjectId { get; set; }
        public string Name { get; set; }

        public List<BookSubject> BookSubjects { get; set; }
    }
}
