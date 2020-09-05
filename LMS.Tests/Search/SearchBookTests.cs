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

namespace LMS.Tests.Search
{
    public class SearchBookTests
    {
        private readonly Mock<IBookRepository> _mockRepo;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly BooksController _controller;
        public SearchBookTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BookProfile());
            });
            var mapper = config.CreateMapper();

            _mockRepo = new Mock<IBookRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _controller = new BooksController(_mockUnitOfWork.Object, mapper);
        }

        [Theory]
        [InlineData("", "", 3)]
        [InlineData("title", "Book", 2)]
        [InlineData("title", "Sample", 1)]
        [InlineData("title", "DoesNotExists", 0)]
        [InlineData("isbn", "", 3)]
        [InlineData("isbn", "123456789", 1)]
        [InlineData("isbn", "123", 3)]
        [InlineData("isbn", "000000000", 0)]
        public void Index_SearchTerm_ReturnsBooksList(string searchBy, string searchTerm, int expectedListSize)
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetAllWithAuthorsSubjects()).Returns(GetTestData());
            _mockUnitOfWork.Setup(uow => uow.Books).Returns(_mockRepo.Object);

            // Act
            var result = _controller.Index(searchBy, searchTerm);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<BookViewModel>>(viewResult.ViewData.Model);
            Assert.Equal(expectedListSize, model.Count());
        }

        private IEnumerable<Book> GetTestData()
        {
            var books = new List<Book>()
            {
                new Book() { BookId = 1, Title = "Book 1", ISBN = "123456789" },
                new Book() { BookId = 2, Title = "Book 2", ISBN = "123123123" },
                new Book() { BookId = 3, Title = "Sample", ISBN = "123111123" },
            };

            return books;
        }
    }
}
