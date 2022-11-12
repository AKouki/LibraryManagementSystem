using LMS.Core.Domain.Issues;

namespace LMS.Web.ViewModels
{
    public class BookViewModel
    {
        public int BookId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Publisher { get; set; }
        public string? Language { get; set; }
        public string? ISBN { get; set; }
        public string? CallNumber { get; set; }

        public List<AuthorViewModel> Authors { get; set; } = new();
        public List<SubjectViewModel> Subjects { get; set; } = new();

        public Issue? Issue { get; set; }
    }
}