using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LMS.Core;
using AutoMapper;
using LMS.Core.Domain.Books;
using LMS.Web.Admin.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace LMS.Web.Admin.Controllers
{
    [Authorize]
    public class AuthorsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AuthorsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IActionResult Index(string searchTerm)
        {
            ViewData["currentFilter"] = searchTerm;

            var authors = _unitOfWork.Authors.GetAllWithBooks();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                authors = authors.Where(a => a.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            var authorsViewModel = _mapper.Map<IEnumerable<Author>, IEnumerable<AuthorViewModel>>(authors);
            return View(authorsViewModel);
        }

        public IActionResult Details(int id)
        {
            var author = _unitOfWork.Authors.GetSingleWithBooks(id);
            if (author == null)
                return NotFound();

            var authorViewModel = _mapper.Map<Author, AuthorViewModel>(author);
            return View(authorViewModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AuthorViewModel authorViewModel)
        {
            if (ModelState.IsValid)
            {
                var author = _mapper.Map<AuthorViewModel, Author>(authorViewModel);
                _unitOfWork.Authors.Add(author);
                _unitOfWork.Save();

                return RedirectToAction("Index");
            }

            return View(authorViewModel);
        }

        public IActionResult Edit(int id)
        {
            var author = _unitOfWork.Authors.Get(id);
            if (author == null)
                return NotFound();

            var authorViewModel = _mapper.Map<Author, AuthorViewModel>(author);
            return View(authorViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AuthorViewModel authorViewModel)
        {
            if (ModelState.IsValid)
            {
                var author = _mapper.Map<AuthorViewModel, Author>(authorViewModel);
                _unitOfWork.Authors.Update(author);
                _unitOfWork.Save();

                return RedirectToAction("Index");
            }

            return View(authorViewModel);
        }

        public IActionResult Delete(int id)
        {
            var author = _unitOfWork.Authors.Get(id);
            if (author == null)
                return NotFound();

            return View(author);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var author = _unitOfWork.Authors.Get(id);
            if (author == null)
                return NotFound();

            _unitOfWork.Authors.Remove(author);
            _unitOfWork.Save();

            return RedirectToAction("Index");
        }
    }
}