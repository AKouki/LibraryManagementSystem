using AutoMapper;
using LMS.Core;
using LMS.Core.Domain.Books;
using LMS.Core.Domain.Issues;
using LMS.Core.Domain.Members;
using LMS.Core.Repositories;
using LMS.Web.Admin.Controllers;
using LMS.Web.Admin.Mappings;
using LMS.Web.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace LMS.Tests.Controllers
{
    public class MembersControllerTests
    {
        private readonly Mock<IMemberRepository> _mockRepo;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly MembersController _controller;

        public MembersControllerTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MemberProfile());
                cfg.AddProfile(new BookProfile());
            });
            var mapper = config.CreateMapper();

            _mockRepo = new Mock<IMemberRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _controller = new MembersController(_mockUnitOfWork.Object, mapper);
        }

        [Fact]
        public void Index_ReturnsAllMembers()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetAll()).Returns(GetTestData());
            _mockUnitOfWork.Setup(uow => uow.Members).Returns(_mockRepo.Object);

            // Act
            var result = _controller.Index("", "");

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<MemberViewModel>>(viewResult.ViewData.Model);
            Assert.Equal(3, model.Count());
        }

        [Fact]
        public void Create_ValidModelState_ReturnsRedirectToIndex()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetAll()).Returns(GetTestData());
            _mockUnitOfWork.Setup(uow => uow.Members).Returns(_mockRepo.Object);
            var member = new MemberViewModel()
            {
                FirstName = "George",
                LastName = "Papadopoulos",
                PhoneNumber = "6945000000"
            };

            // Act
            var result = _controller.Create(member);

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", viewResult.ActionName);
        }

        [Fact]
        public void Create_InvalidModelState_ReturnsView()
        {
            // Arrange
            var member = new MemberViewModel() { FirstName = "George", LastName = "Papadopoulos" };
            _controller.ModelState.AddModelError("Phone", "Required");

            // Act
            var result = _controller.Create(member);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<MemberViewModel>(viewResult.Model);
            Assert.Equal(model.FirstName, member.FirstName);
            Assert.Equal(model.LastName, member.LastName);
            Assert.Equal(model.PhoneNumber, member.PhoneNumber);
        }

        [Fact]
        public void Details_ValidId_ReturnsDetailsAndIssuedBooks()
        {
            // Arrange
            var memberId = 1;
            _mockRepo.Setup(repo => repo.GetMemberWithIssuedBooks(memberId)).Returns(GetTestData().FirstOrDefault());
            _mockUnitOfWork.Setup(uow => uow.Members).Returns(_mockRepo.Object);

            // Act
            var result = _controller.Details(memberId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<MemberViewModel>(viewResult.Model);
            Assert.Equal(1, model.Issues.Count);
        }

        [Fact]
        public void Details_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var memberId = 0;
            _mockRepo.Setup(repo => repo.GetMemberWithIssuedBooks(memberId)).Returns((Member)null);
            _mockUnitOfWork.Setup(uow => uow.Members).Returns(_mockRepo.Object);

            // Act
            var result = _controller.Details(memberId);

            // Assert
            var viewResult = Assert.IsType<NotFoundResult>(result);
        }

        private IEnumerable<Member> GetTestData()
        {
            var books = new List<Book>()
            {
                new Book() { BookId = 1, Title = "Book 1" },
                new Book() { BookId = 2, Title = "Book 2" },
            };

            var members = new List<Member>()
            {
                new Member(){ MemberId = 1, FirstName = "George", LastName = "Papas", PhoneNumber = "0000000000", MemberType = MemberType.Student },
                new Member(){ MemberId = 1, FirstName = "George", LastName = "Papadopoulos", PhoneNumber = "0000000001", MemberType = MemberType.Student },
                new Member(){ MemberId = 1, FirstName = "Maria", LastName = "Georgiou", PhoneNumber = "0000000002", MemberType = MemberType.Student }
            };

            var issuedBooks = new List<Issue>()
            {
                new Issue()
                {
                    Member = members[0],
                    Book = books[0]
                }
            };

            members[0].Issues = issuedBooks;

            return members;
        }
    }
}
