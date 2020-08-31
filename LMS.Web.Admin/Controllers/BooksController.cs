using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LMS.Data;
using LMS.Core;
using LMS.Core.Domain.Books;
using LMS.Web.Admin.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LMS.Web.Admin.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public BooksController(IUnitOfWork unitOfWork, IMapper mapper)
        {
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

        public IActionResult Create()
        {
            var bookViewModel = new BookViewModel();
            bookViewModel.AuthorsItems = _unitOfWork.Authors.GetAll().Select(s => new SelectListItem() { Text = s.Name, Value = s.AuthorId.ToString() });
            bookViewModel.SubjectsItems = _unitOfWork.Subjects.GetAll().Select(s => new SelectListItem() { Text = s.Name, Value = s.SubjectId.ToString() });

            return View(bookViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BookViewModel bookViewModel)
        {
            if (ModelState.IsValid)
            {
                var book = _mapper.Map<BookViewModel, Book>(bookViewModel);

                var bookAuthors = new List<BookAuthor>();
                var bookSubjects = new List<BookSubject>();

                if (bookViewModel.SelectedAuthors != null)
                {
                    var selectedAuthors = _unitOfWork.Authors.Find(a => bookViewModel.SelectedAuthors.Contains(a.AuthorId)).ToList();
                    foreach (var item in selectedAuthors)
                    {
                        bookAuthors.Add(new BookAuthor()
                        {
                            Book = book,
                            Author = item
                        });
                    }
                }

                if (bookViewModel.SelectedSubjects != null)
                {
                    var selectedSubjects = _unitOfWork.Subjects.Find(a => bookViewModel.SelectedSubjects.Contains(a.SubjectId)).ToList();
                    foreach (var item in selectedSubjects)
                    {
                        bookSubjects.Add(new BookSubject()
                        {
                            Book = book,
                            Subject = item
                        });
                    }
                }

                book.BookAuthors = bookAuthors;
                book.BookSubjects = bookSubjects;

                _unitOfWork.Books.Add(book);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }

            return View(bookViewModel);
        }

        public IActionResult Edit(int id)
        {
            var book = _unitOfWork.Books.Get(id);
            if (book == null)
                return NotFound();

            var bookViewModel = _mapper.Map<Book, BookViewModel>(book);
            return View(bookViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(BookViewModel bookViewModel)
        {
            if (ModelState.IsValid)
            {
                var book = _mapper.Map<BookViewModel, Book>(bookViewModel);
                _unitOfWork.Books.Update(book);
                _unitOfWork.Save();

                return RedirectToAction("Index");
            }

            return View(bookViewModel);
        }

        public IActionResult Delete(int id)
        {
            var book = _unitOfWork.Books.Get(id);
            if (book == null)
                return NotFound();

            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var book = _unitOfWork.Books.Get(id);
            if (book == null)
                return NotFound();

            _unitOfWork.Books.Remove(book);
            _unitOfWork.Save();

            return RedirectToAction("Index");
        }
    }
}