namespace LMS.Web.ViewModels
{
    public class AuthorViewModel
    {
        public int AuthorId { get; set; }
        public string? Name { get; set; }
        public List<BookViewModel> Books { get; set; } = new();
    }
}
