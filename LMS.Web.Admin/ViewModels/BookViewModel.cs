using LMS.Core.Domain.Books;
using LMS.Core.Domain.Issues;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LMS.Web.Admin.ViewModels
{
    public class BookViewModel
    {
        public BookViewModel()
        {
            this.Authors = new HashSet<Author>();
            this.Subjects = new HashSet<Subject>();
            this.Language = "English";
            this.MaxIssueDays = 10;
        }

        [DisplayName("Id")]
        public int BookId { get; set; }

        [Required]
        [StringLength(150, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public string Publisher { get; set; }

        [Required]
        public string Language { get; set; }

        [Required]
        public string ISBN { get; set; }

        [DisplayName("Call Number")]
        public string CallNumber { get; set; }

        [DisplayName("Max Issue Days")]
        public int MaxIssueDays { get; set; }

        public ICollection<Author> Authors { get; set; }
        public int[] SelectedAuthors { get; set; }
        public IEnumerable<SelectListItem> AuthorsItems { get; set; }

        public ICollection<Subject> Subjects { get; set; }
        public int[] SelectedSubjects { get; set; }
        public IEnumerable<SelectListItem> SubjectsItems { get; set; }

        public Issue Issue { get; set; }
    }
}
