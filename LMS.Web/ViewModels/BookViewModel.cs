using LMS.Core.Domain.Books;
using LMS.Core.Domain.Issues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Web.ViewModels
{
    public class BookViewModel
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Publisher { get; set; }
        public string Language { get; set; }
        public string ISBN { get; set; }
        public string CallNumber { get; set; }

        public List<AuthorViewModel> Authors { get; set; }
        public List<SubjectViewModel> Subjects { get; set; }

        public Issue Issue { get; set; }
    }
}
