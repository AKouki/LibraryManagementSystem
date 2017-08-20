using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LMS.Web.Admin.ViewModels
{
    public class AuthorViewModel
    {
        [DisplayName("Id")]
        public int AuthorId { get; set; }

        [Required]
        public string Name { get; set; }

        public List<BookViewModel> Books { get; set; }
    }
}
