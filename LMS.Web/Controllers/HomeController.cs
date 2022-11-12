using AutoMapper;
using LMS.Core;
using LMS.Core.Domain.Books;
using LMS.Web.Models;
using LMS.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LMS.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<HomeController> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public IActionResult Index(string searchBy, string searchTerm)
        {
            ViewData["currentFilter"] = searchTerm;

            var books = _unitOfWork.Books.GetAllWithAuthorsSubjects();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                if (searchBy.Equals("title"))
                    books = books.Where(b => b.Title!.ToLower().Contains(searchTerm.ToLower())).ToList();
                else if (searchBy.Equals("isbn"))
                    books = books.Where(b => b.ISBN!.ToLower().Contains(searchTerm.ToLower())).ToList();
            }

            var booksViewModel = _mapper.Map<IEnumerable<Book>, IEnumerable<BookViewModel>>(books);
            return View(booksViewModel);
        }

        public IActionResult Details(int id)
        {
            var book = _unitOfWork.Books.GetSingleWithAuthorsSubjects(id);
            if (book == null)
                return NotFound();

            var bookViewModel = _mapper.Map<Book, BookViewModel>(book);
            return View(bookViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}