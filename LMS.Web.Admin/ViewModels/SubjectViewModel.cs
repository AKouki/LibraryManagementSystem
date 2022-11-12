using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace LMS.Web.Admin.ViewModels
{
    public class SubjectViewModel
    {
        [DisplayName("Id")]
        public int SubjectId { get; set; }

        [Required]
        public string? Name { get; set; }

        public List<BookViewModel> Books { get; set; } = new();
    }
}
