using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LMS.Web.Models;
using LMS.Core;
using AutoMapper;
using LMS.Core.Domain.Books;
using LMS.Web.ViewModels;

namespace LMS.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IActionResult Index(string searchBy, string searchTerm)
        {
            ViewData["currentFilter"] = searchTerm;

            var books = _unitOfWork.Books.GetAllWithAuthorsSubjects();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                if (searchBy.Equals("title"))
                    books = books.Where(b => b.Title.ToLower().Contains(searchTerm.ToLower())).ToList();
                else if (searchBy.Equals("isbn"))
                    books = books.Where(b => b.ISBN.ToLower().Contains(searchTerm.ToLower())).ToList();
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
