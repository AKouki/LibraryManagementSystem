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
    public class SearchSubjectTests
    {
        private readonly Mock<ISubjectRepository> _mockRepo;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly SubjectsController _controller;

        public SearchSubjectTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new SubjectProfile());
            });
            var mapper = config.CreateMapper();

            _mockRepo = new Mock<ISubjectRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _controller = new SubjectsController(_mockUnitOfWork.Object, mapper);
        }

        [Theory]
        [InlineData("", 3)]
        [InlineData("Subject", 2)]
        [InlineData("Subject 1", 1)]
        [InlineData("Subject123", 0)]
        public void Index_SearchTerm_ReturnsSubjectsList(string searchTerm, int expectedListSize)
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetAllWithBooks()).Returns(GetTestData());
            _mockUnitOfWork.Setup(uow => uow.Subjects).Returns(_mockRepo.Object);

            // Act
            var result = _controller.Index(searchTerm);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<SubjectViewModel>>(viewResult.ViewData.Model);
            Assert.Equal(expectedListSize, model.Count());
        }

        private IEnumerable<Subject> GetTestData()
        {
            var subjects = new List<Subject>()
            {
                new Subject() { SubjectId = 1, Name = "Subject 1" },
                new Subject() { SubjectId = 2, Name = "Subject 2" },
                new Subject() { SubjectId = 3, Name = "Sample" }
            };

            return subjects;
        }
    }
}
