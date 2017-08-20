using LMS.Core.Domain.Issues;
using LMS.Core.Domain.Members;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Core.Domain.Books
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Publisher { get; set; }
        public string Language { get; set; }
        public string ISBN { get; set; }
        public string CallNumber { get; set; }
        public int MaxIssueDays { get; set; }
        
        public List<BookAuthor> BookAuthors { get; set; }
        public List<BookSubject> BookSubjects { get; set; }

        public Issue Issue { get; set; }
    }
}
