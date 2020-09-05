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
    public class BooksControllerTests
    {
        private readonly Mock<IBookRepository> _mockRepo;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly BooksController _controller;

        public BooksControllerTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BookProfile());
                cfg.AddProfile(new AuthorProfile());
                cfg.AddProfile(new SubjectProfile());
            });
            var mapper = config.CreateMapper();

            _mockRepo = new Mock<IBookRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _controller = new BooksController(_mockUnitOfWork.Object, mapper);
        }

        [Fact]
        public void Index_ReturnsAllBooks()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetAllWithAuthorsSubjects()).Returns(GetTestData());
            _mockUnitOfWork.Setup(uow => uow.Books).Returns(_mockRepo.Object);

            // Act
            var result = _controller.Index("", "");

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<BookViewModel>>(viewResult.ViewData.Model);
            Assert.Equal(3, model.Count());
        }

        [Fact]
        public void Create_ValidModelState_ReturnsRedirectToIndex()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetAll()).Returns(GetTestData());
            _mockUnitOfWork.Setup(uow => uow.Books).Returns(_mockRepo.Object);
            var book = new BookViewModel()
            {
                Title = "New Book",
                Publisher = "ABC Publisher",
                Language = "English",
                ISBN = "123456789"
            };

            // Act
            var result = _controller.Create(book);

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", viewResult.ActionName);
        }

        [Fact]
        public void Create_InvalidModelState_ReturnsView()
        {
            // Arrange
            var book = new BookViewModel()
            {
                Title = "New Book",
                Publisher = "ABC Publisher"
            };
            _controller.ModelState.AddModelError("Language", "Required");
            _controller.ModelState.AddModelError("ISBN", "Required");

            // Act
            var result = _controller.Create(book);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<BookViewModel>(viewResult.Model);
            Assert.Equal(model.Title, book.Title);
            Assert.Equal(model.Publisher, book.Publisher);
            Assert.Equal(model.Language, book.Language);
            Assert.Equal(model.ISBN, book.ISBN);
        }

        [Fact]
        public void Edit_ValidModelState_ReturnsRedirectToIndex()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetAll()).Returns(GetTestData());
            _mockUnitOfWork.Setup(uow => uow.Books).Returns(_mockRepo.Object);
            var book = new BookViewModel()
            {
                BookId = 1,
                Title = "New Title",
                Publisher = "ABC Publisher",
                Language = "English",
                ISBN = "123456789"
            };

            // Act
            var result = _controller.Edit(book);

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", viewResult.ActionName);
        }

        [Fact]
        public void Edit_InvalidModelState_ReturnsView()
        {
            // Arrange
            var book = new BookViewModel()
            {
                Title = "New Book",
                Publisher = "ABC Publisher",
            };
            _controller.ModelState.AddModelError("Language", "Required");
            _controller.ModelState.AddModelError("ISBN", "Required");

            // Act
            var result = _controller.Edit(book);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<BookViewModel>(viewResult.Model);
            Assert.Equal(model.Title, book.Title);
            Assert.Equal(model.Publisher, book.Publisher);
            Assert.Equal(model.Language, book.Language);
            Assert.Equal(model.ISBN, book.ISBN);
        }

        [Fact]
        public void Details_ValidId_ReturnsDetailsAndIssuer()
        {
            // Arrange
            var bookId = 1;
            _mockRepo.Setup(repo => repo.GetSingleWithAuthorsSubjects(bookId)).Returns(GetTestData().FirstOrDefault());
            _mockUnitOfWork.Setup(uow => uow.Books).Returns(_mockRepo.Object);

            // Act
            var result = _controller.Details(bookId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<BookViewModel>(viewResult.Model);
            Assert.NotNull(model.Issue);
        }

        [Fact]
        public void Details_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var bookId = 0;
            _mockRepo.Setup(repo => repo.GetSingleWithAuthorsSubjects(bookId)).Returns((Book)null);
            _mockUnitOfWork.Setup(uow => uow.Books).Returns(_mockRepo.Object);

            // Act
            var result = _controller.Details(bookId);

            // Assert
            var viewResult = Assert.IsType<NotFoundResult>(result);
        }

        private IEnumerable<Book> GetTestData()
        {
            var member = new Member()
            {
                MemberId = 1,
                FirstName = "George",
                LastName = "Papas",
                PhoneNumber = "0000000000",
                MemberType = MemberType.Student
            };

            var books = new List<Book>()
            {
                new Book() { BookId = 1, Title = "Book 1", ISBN = "123456789" },
                new Book() { BookId = 2, Title = "Book 2", ISBN = "123123123" },
                new Book() { BookId = 3, Title = "Sample", ISBN = "123111123" },
            };

            var issue = new Issue()
            {
                Book = books[0],
                Member = member
            };

            books[0].Issue = issue;

            return books;
        }
    }
}
