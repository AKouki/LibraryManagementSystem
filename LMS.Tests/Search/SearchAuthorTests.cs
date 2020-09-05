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
    public class SearchAuthorTests
    {
        private readonly Mock<IAuthorRepository> _mockRepo;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly AuthorsController _controller;
        public SearchAuthorTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AuthorProfile());
            });
            var mapper = config.CreateMapper();

            _mockRepo = new Mock<IAuthorRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _controller = new AuthorsController(_mockUnitOfWork.Object, mapper);
        }

        [Theory]
        [InlineData("", 3)]
        [InlineData("Author", 2)]
        [InlineData("Author 1", 1)]
        [InlineData("Author123", 0)]
        public void Index_SearchTerm_ReturnsAuthorsList(string searchTerm, int expectedListSize)
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetAllWithBooks()).Returns(GetTestData());
            _mockUnitOfWork.Setup(uow => uow.Authors).Returns(_mockRepo.Object);

            // Act
            var result = _controller.Index(searchTerm);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<AuthorViewModel>>(viewResult.ViewData.Model);
            Assert.Equal(expectedListSize, model.Count());
        }

        private IEnumerable<Author> GetTestData()
        {
            var authors = new List<Author>()
            {
                new Author() { AuthorId = 1, Name = "Author 1" },
                new Author() { AuthorId = 2, Name = "Author 2" },
                new Author() { AuthorId = 3, Name = "Sample" }
            };

            return authors;
        }
    }
}
