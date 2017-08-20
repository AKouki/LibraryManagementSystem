using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Web.ViewModels
{
    public class AuthorViewModel
    {
        public int AuthorId { get; set; }
        public string Name { get; set; }
        public List<BookViewModel> Books { get; set; }
    }
}
