using AutoMapper;
using LMS.Core;
using LMS.Core.Domain.Issues;
using LMS.Web.Admin.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LMS.Web.Admin.Controllers
{
    [Authorize]
    public class IssuesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public IssuesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IActionResult Index(string searchBy, string searchTerm)
        {
            ViewData["currentFilter"] = searchTerm;

            var issuedBooks = _unitOfWork.Issues.GetAllIssuedBooks();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                if (searchBy.Equals("bookTitle"))
                    issuedBooks = issuedBooks.Where(b => b.Book.Title.ToLower().Contains(searchTerm.ToLower())).ToList();
                else if (searchBy.Equals("memberPhone"))
                    issuedBooks = issuedBooks.Where(b => b.Member.PhoneNumber.Equals(searchTerm));
            }

            var issuedBooksViewModel = _mapper.Map<IEnumerable<Issue>, IEnumerable<IssueReturnViewModel>>(issuedBooks);
            return View(issuedBooksViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Issue(int memberId, int bookId)
        {
            if (IsAvailable(bookId))
            {
                var member = _unitOfWork.Members.Get(memberId);
                var book = _unitOfWork.Books.Get(bookId);
                if (member == null || book == null)
                    return Content("This member or book does not exist!");

                var newIssue = new Issue
                {
                    Member = member,
                    Book = book,
                    IssueDate = DateTime.Now,
                    ExpireDate = DateTime.Now.AddDays(book.MaxIssueDays)
                };

                _unitOfWork.Issues.Add(newIssue);
                _unitOfWork.Save();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Return(int bookId)
        {
            var issue = _unitOfWork.Issues.GetByBookId(bookId);
            if (issue == null)
                return Content("This book is not issued to any member!");

            _unitOfWork.Issues.Remove(issue);
            _unitOfWork.Save();

            return RedirectToAction("Index");
        }

        private bool IsAvailable(int bookId)
        {
            return _unitOfWork.Issues.GetByBookId(bookId) == null;
        }
    }
}