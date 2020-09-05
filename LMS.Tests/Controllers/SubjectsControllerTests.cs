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
    public class SubjectsControllerTests
    {
        private readonly Mock<ISubjectRepository> _mockRepo;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly SubjectsController _controller;

        public SubjectsControllerTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new SubjectProfile());
                cfg.AddProfile(new BookProfile());
            });
            var mapper = config.CreateMapper();

            _mockRepo = new Mock<ISubjectRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _controller = new SubjectsController(_mockUnitOfWork.Object, mapper);
        }

        [Fact]
        public void Index_ReturnsAllSubjects()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetAllWithBooks()).Returns(GetTestData());
            _mockUnitOfWork.Setup(uow => uow.Subjects).Returns(_mockRepo.Object);

            // Act
            var result = _controller.Index("");

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<SubjectViewModel>>(viewResult.ViewData.Model);
            Assert.Equal(5, model.Count());
        }

        [Fact]
        public void Create_ValidModelState_ReturnsRedirectToIndex()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetAll()).Returns(GetTestData());
            _mockUnitOfWork.Setup(uow => uow.Subjects).Returns(_mockRepo.Object);
            var subject = new SubjectViewModel() { Name = "Subject" };

            // Act
            var result = _controller.Create(subject);

            // Assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", viewResult.ActionName);
        }

        [Fact]
        public void Create_InvalidModelState_ReturnsView()
        {
            // Arrange
            var subject = new SubjectViewModel() { };
            _controller.ModelState.AddModelError("Title", "Required");

            // Act
            var result = _controller.Create(subject);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<SubjectViewModel>(viewResult.Model);
            Assert.Equal(model.Name, subject.Name);
        }

        [Fact]
        public void Details_ValidId_ReturnsDetailsAndSubjectBooks()
        {
            // Arrange
            var subjectId = 1;
            _mockRepo.Setup(repo => repo.GetSingleWithBooks(subjectId)).Returns(GetTestData().FirstOrDefault());
            _mockUnitOfWork.Setup(uow => uow.Subjects).Returns(_mockRepo.Object);

            // Act
            var result = _controller.Details(subjectId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<SubjectViewModel>(viewResult.Model);
            Assert.Equal(2, model.Books.Count);
        }

        [Fact]
        public void Details_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var subjectId = 0;
            _mockRepo.Setup(repo => repo.GetSingleWithBooks(subjectId)).Returns((Subject)null);
            _mockUnitOfWork.Setup(uow => uow.Subjects).Returns(_mockRepo.Object);

            // Act
            var result = _controller.Details(subjectId);

            // Assert
            var viewResult = Assert.IsType<NotFoundResult>(result);
        }

        private IEnumerable<Subject> GetTestData()
        {
            var books = new List<Book>()
            {
                new Book() { BookId = 1, Title = "Book 1" },
                new Book() { BookId = 2, Title = "Book 2" },
            };

            var subjects = new List<Subject>()
            {
                new Subject() { SubjectId = 1, Name = "Subject 1" },
                new Subject() { SubjectId = 2, Name = "Subject 2" },
                new Subject() { SubjectId = 3, Name = "Subject 3" },
                new Subject() { SubjectId = 4, Name = "Subject 4" },
                new Subject() { SubjectId = 5, Name = "Subject 5" }
            };

            var bookSubjects = new List<BookSubject>()
            {
                new BookSubject() { Book = books[0], Subject = subjects[0] },
                new BookSubject() { Book = books[1], Subject = subjects[0] },
            };

            subjects[0].BookSubjects = bookSubjects;

            return subjects;
        }
    }
}
