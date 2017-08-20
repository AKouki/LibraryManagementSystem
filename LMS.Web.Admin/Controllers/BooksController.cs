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

namespace LMS.Web.Admin.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        IUnitOfWork _unitOfWork;
        IMapper _mapper;
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

        public IActionResult Details(int? id)
        {
            if (id == null)
                return BadRequest();

            var book = _unitOfWork.Books.GetSingleWithAuthorsSubjects((int)id);
            if (book == null)
                return NotFound();

            var bookViewModel = _mapper.Map<Book, BookViewModel>(book);
            return View(bookViewModel);
        }

        public IActionResult Create()
        {
            var bookViewModel = new BookViewModel();
            bookViewModel.Create(2);

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

                foreach (var item in bookViewModel.Authors)
                {
                    if (!string.IsNullOrEmpty(item.Name))
                    {
                        var author = _unitOfWork.Authors
                            .Find(a => a.Name == item.Name)
                            .FirstOrDefault();

                        bookAuthors.Add(new BookAuthor
                        {
                            Book = book,
                            Author = author == null ? item : author
                        });
                    }
                }

                foreach (var item in bookViewModel.Subjects)
                {
                    if (!string.IsNullOrEmpty(item.Name))
                    {
                        var subject = _unitOfWork.Subjects
                        .Find(s => s.Name == item.Name)
                        .FirstOrDefault();

                        bookSubjects.Add(new BookSubject
                        {
                            Book = book,
                            Subject = subject == null ? item : subject
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

        public IActionResult Edit(int? id)
        {
            if (id == null)
                return BadRequest();

            var book = _unitOfWork.Books.Get((int)id);
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

        public IActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var book = _unitOfWork.Books.Get((int)id);
            if (book == null)
                return NotFound();

            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var book = _unitOfWork.Books.Get(id);
            _unitOfWork.Books.Remove(book);
            _unitOfWork.Save();

            return RedirectToAction("Index");
        }
    }
}