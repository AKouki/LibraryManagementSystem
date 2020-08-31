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
        [StringLength(150, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
        [DisplayName("Author Name")]
        public string Name { get; set; }

        public List<BookViewModel> Books { get; set; }
    }
}
