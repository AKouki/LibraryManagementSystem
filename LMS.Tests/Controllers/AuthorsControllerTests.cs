using AutoMapper;
using LMS.Core;
using LMS.Core.Domain.Books;
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
    public class AuthorsControllerTests
    {
        private readonly Mock<IAuthorRepository> _mockRepo;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly AuthorsController _controller;

        public AuthorsControllerTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AuthorProfile());
                cfg.AddProfile(new BookProfile());
            });
            var mapper = config.CreateMapper();

            _mockRepo = new Mock<IAuthorRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _controller = new AuthorsController(_mockUnitOfWork.Object, mapper);
        }

        [Fact]
        public void Index_ReturnsAllAuthors()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetAllWithBooks()).Returns(GetTestData());
            _mockUnitOfWork.Setup(uow => uow.Authors).Returns(_mockRepo.Object);

            // Act
            var result = _controller.Index("");

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<AuthorViewModel>>(viewResult.ViewData.Model);
            Assert.Equal(3, model.Count());
        }

        [Fact]
        public void Create_ValidModelState_ReturnsRedirectToIndex()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetAll()).Returns(GetTestData());
            _mockUnitOfWork.Setup(uow => uow.Authors).Returns(_mockRepo.Object);
            var author = new AuthorViewModel() { Name = "Author" };

            // Act
            var result = _controller.Create(author);

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", viewResult.ActionName);
        }

        [Fact]
        public void Create_InvalidModelState_ReturnsView()
        {
            // Arrange
            var author = new AuthorViewModel() { };
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = _controller.Create(author);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<AuthorViewModel>(viewResult.Model);
            Assert.Equal(model.Name, author.Name);
        }

        [Fact]
        public void Details_ValidId_ReturnsDetailsAndAuthorBooks()
        {
            // Arrange
            var authorId = 1;
            _mockRepo.Setup(repo => repo.GetSingleWithBooks(authorId)).Returns(GetTestData().FirstOrDefault());
            _mockUnitOfWork.Setup(uow => uow.Authors).Returns(_mockRepo.Object);

            // Act
            var result = _controller.Details(authorId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<AuthorViewModel>(viewResult.Model);
            Assert.Equal(2, model.Books.Count);
        }

        [Fact]
        public void Details_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var authorId = 0;
            _mockRepo.Setup(repo => repo.GetSingleWithBooks(authorId)).Returns((Author)null);
            _mockUnitOfWork.Setup(uow => uow.Authors).Returns(_mockRepo.Object);

            // Act
            var result = _controller.Details(authorId);

            // Assert
            var viewResult = Assert.IsType<NotFoundResult>(result);
        }

        private IEnumerable<Author> GetTestData()
        {
            var books = new List<Book>()
            {
                new Book() { BookId = 1, Title = "Book 1" },
                new Book() { BookId = 2, Title = "Book 2" },
            };

            var authors = new List<Author>()
            {
                new Author() { AuthorId = 1, Name = "Author 1" },
                new Author() { AuthorId = 2, Name = "Author 2" },
                new Author() { AuthorId = 3, Name = "Sample" }
            };

            var bookAuthors = new List<BookAuthor>()
            {
                new BookAuthor() { Book = books[0], Author = authors[0] },
                new BookAuthor() { Book = books[1], Author = authors[0] },
            };

            authors[0].BookAuthors = bookAuthors;

            return authors;
        }
    }
}
