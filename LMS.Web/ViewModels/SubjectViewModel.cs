namespace LMS.Web.ViewModels
{
    public class SubjectViewModel
    {
        public int SubjectId { get; set; }
        public string? Name { get; set; }
        public List<BookViewModel> Books { get; set; } = new();
    }
}