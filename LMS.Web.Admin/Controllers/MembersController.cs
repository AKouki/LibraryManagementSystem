using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using LMS.Core;
using LMS.Core.Domain.Members;
using LMS.Web.Admin.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace LMS.Web.Admin.Controllers
{
    [Authorize]
    public class MembersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public MembersController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IActionResult Index(string searchBy, string searchTerm)
        {
            ViewData["currentFilter"] = searchTerm;

            var members = _unitOfWork.Members.GetAll();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                if (searchBy.Equals("firstname"))
                    members = members.Where(m => m.FirstName.ToLower().Contains(searchTerm.ToLower())).ToList();
                else if (searchBy.Equals("lastname"))
                    members = members.Where(m => m.LastName.ToLower().Contains(searchTerm.ToLower())).ToList();
                else if (searchBy.Equals("phone"))
                    members = members.Where(m => m.PhoneNumber.ToLower().Contains(searchTerm.ToLower())).ToList();
            }

            var membersViewModel = _mapper.Map<IEnumerable<Member>, IEnumerable<MemberViewModel>>(members);
            return View(membersViewModel);
        }

        public IActionResult Details(int id)
        {
            var member = _unitOfWork.Members.GetMemberWithIssuedBooks(id);
            if (member == null)
                return NotFound();

            var memberViewModel = _mapper.Map<Member, MemberViewModel>(member);
            return View(memberViewModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(MemberViewModel memberViewModel)
        {
            if (ModelState.IsValid)
            {
                var member = _mapper.Map<MemberViewModel, Member>(memberViewModel);
                _unitOfWork.Members.Add(member);
                _unitOfWork.Save();

                return RedirectToAction("Index");
            }

            return View(memberViewModel);
        }

        public IActionResult Edit(int id)
        {
            var member = _unitOfWork.Members.Get(id);
            if (member == null)
                return NotFound();

            var memberViewModel = _mapper.Map<Member, MemberViewModel>(member);
            return View(memberViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(MemberViewModel memberViewModel)
        {
            if (ModelState.IsValid)
            {
                var member = _mapper.Map<MemberViewModel, Member>(memberViewModel);
                _unitOfWork.Members.Update(member);
                _unitOfWork.Save();

                return RedirectToAction("Index");
            }

            return View(memberViewModel);
        }

        public IActionResult Delete(int id)
        {
            var member = _unitOfWork.Members.Get(id);
            if (member == null)
                return NotFound();

            return View(member);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var member = _unitOfWork.Members.Get(id);
            if (member == null)
                return NotFound();

            _unitOfWork.Members.Remove(member);
            _unitOfWork.Save();

            return RedirectToAction("Index");
        }
    }
}