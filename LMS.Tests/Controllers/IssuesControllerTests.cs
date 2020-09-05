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
    public class IssuesControllerTests
    {
        private readonly Mock<ITransactionRepository> _mockRepo;
        private readonly Mock<IMemberRepository> _mockMemberRepo;
        private readonly Mock<IBookRepository> _mockBookRepo;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly IssuesController _controller;

        public IssuesControllerTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new IssueReturnProfile());
                cfg.AddProfile(new BookProfile());
                cfg.AddProfile(new MemberProfile());
            });
            var mapper = config.CreateMapper();

            _mockRepo = new Mock<ITransactionRepository>();
            _mockMemberRepo = new Mock<IMemberRepository>();
            _mockBookRepo = new Mock<IBookRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _controller = new IssuesController(_mockUnitOfWork.Object, mapper);

            _mockUnitOfWork.Setup(uow => uow.Members).Returns(_mockMemberRepo.Object);
            _mockUnitOfWork.Setup(uow => uow.Books).Returns(_mockBookRepo.Object);
        }

        [Fact]
        public void Index_ReturnsListWithBorrowedBooks()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetAllIssuedBooks()).Returns(GetTestData());
            _mockUnitOfWork.Setup(uow => uow.Issues).Returns(_mockRepo.Object);

            // Act
            var result = _controller.Index("", "");

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<IssueReturnViewModel>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public void Issue_InvalidMemberId_ReturnsNotFound()
        {
            // Arrange
            var memberId = 0;
            var bookId = 3;
            _mockMemberRepo.Setup(repo => repo.Get(memberId)).Returns((Member)null);
            _mockBookRepo.Setup(repo => repo.Get(bookId)).Returns(GetTestData().FirstOrDefault().Book);
            _mockRepo.Setup(repo => repo.GetAllIssuedBooks()).Returns(GetTestData());
            _mockRepo.Setup(repo => repo.GetByBookId(bookId)).Returns((Issue)null);
            _mockUnitOfWork.Setup(uow => uow.Issues).Returns(_mockRepo.Object);

            // Act
            var result = _controller.Issue(memberId, bookId);

            // Assert
            var viewResult = Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void Issue_InvalidBookId_ReturnsNotFound()
        {
            // Arrange
            var memberId = 1;
            var bookId = 0;
            _mockMemberRepo.Setup(repo => repo.Get(memberId)).Returns(GetTestData().FirstOrDefault().Member);
            _mockBookRepo.Setup(repo => repo.Get(bookId)).Returns((Book)null);
            _mockRepo.Setup(repo => repo.GetAllIssuedBooks()).Returns(GetTestData());
            _mockRepo.Setup(repo => repo.GetByBookId(bookId)).Returns((Issue)null);
            _mockUnitOfWork.Setup(uow => uow.Issues).Returns(_mockRepo.Object);

            // Act
            var result = _controller.Issue(memberId, bookId);

            // Assert
            var viewResult = Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void Issue_ValidModel_ReturnsRedirectToIndex()
        {
            // Arrange
            var memberId = 1;
            var bookId = 3;
            _mockMemberRepo.Setup(repo => repo.Get(memberId)).Returns(GetTestData().FirstOrDefault().Member);
            _mockBookRepo.Setup(repo => repo.Get(bookId)).Returns(GetTestData().FirstOrDefault().Book);
            _mockRepo.Setup(repo => repo.GetAllIssuedBooks()).Returns(GetTestData());
            _mockRepo.Setup(repo => repo.GetByBookId(bookId)).Returns((Issue)null);
            _mockUnitOfWork.Setup(uow => uow.Issues).Returns(_mockRepo.Object);

            // Act
            var result = _controller.Issue(memberId, bookId);

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", viewResult.ActionName);
        }

        [Fact]
        public void Return_InvalidBookId_ReturnsNotFound()
        {
            // Arrange
            var bookId = 3;
            _mockRepo.Setup(repo => repo.GetByBookId(bookId)).Returns((Issue)null);
            _mockUnitOfWork.Setup(uow => uow.Issues).Returns(_mockRepo.Object);

            // Act
            var result = _controller.Return(bookId);

            // Assert
            var viewResult = Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void Return_ValidBookId_ReturnsRedirectToIndex()
        {
            // Arrange
            var bookId = 3;
            _mockRepo.Setup(repo => repo.GetByBookId(bookId)).Returns(GetTestData().FirstOrDefault());
            _mockUnitOfWork.Setup(uow => uow.Issues).Returns(_mockRepo.Object);

            // Act
            var result = _controller.Return(bookId);

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", viewResult.ActionName);
        }

        private IEnumerable<Issue> GetTestData()
        {
            var members = new List<Member>()
            {
                new Member() { MemberId = 1, FirstName = "George", LastName = "Papas", PhoneNumber = "0000000000", MemberType = MemberType.Student },
                new Member() { MemberId = 2, FirstName = "George", LastName = "Papadopoulos", PhoneNumber = "0000000001", MemberType = MemberType.Student }
            };

            var books = new List<Book>()
            {
                new Book() { BookId = 1, Title = "Book 1", ISBN = "123456789" },
                new Book() { BookId = 2, Title = "Book 2", ISBN = "123123123" },
                new Book() { BookId = 3, Title = "Sample", ISBN = "123111123" },
            };

            var issuedBooks = new List<Issue>()
            {
                new Issue() { Book = books[0], Member = members[0] },
                new Issue() { Book = books[1], Member = members[1] }
            };

            return issuedBooks;
        }
    }
}
