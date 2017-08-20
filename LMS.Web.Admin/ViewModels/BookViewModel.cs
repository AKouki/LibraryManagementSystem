using LMS.Core.Domain.Books;
using LMS.Core.Domain.Issues;
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

            // Initial values
            this.Language = "English";
            this.MaxIssueDays = 10;
        }

        [DisplayName("Id")]
        public int BookId { get; set; }

        [Required]
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
        public ICollection<Subject> Subjects { get; set; }

        public Issue Issue { get; set; }

        internal void Create(int count = 1)
        {
            for (int i = 0; i < count; i++)
            {
                this.Authors.Add(new Author());
                this.Subjects.Add(new Subject());
            }
        }
    }
}
