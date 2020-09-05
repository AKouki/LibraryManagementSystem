using AutoMapper;
using LMS.Core;
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

namespace LMS.Tests.Search
{
    public class SearchMemberTests
    {
        private readonly Mock<IMemberRepository> _mockRepo;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly MembersController _controller;

        public SearchMemberTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MemberProfile());
            });
            var mapper = config.CreateMapper();

            _mockRepo = new Mock<IMemberRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _controller = new MembersController(_mockUnitOfWork.Object, mapper);
        }

        [Theory]
        [InlineData("", "", 3)]
        [InlineData("firstname", "", 3)]
        [InlineData("firstname", "George", 2)]
        [InlineData("firstname", "DoesNotExists", 0)]
        [InlineData("lastname", "", 3)]
        [InlineData("lastname", "Papa", 2)]
        [InlineData("lastname", "Papadopoulos", 1)]
        [InlineData("lastname", "DoesNotExists", 0)]
        [InlineData("phone", "", 3)]
        [InlineData("phone", "0000000001", 1)]
        [InlineData("phone", "0000000005", 0)]

        public void Index_SearchTerm_ReturnsMembersList(string searchBy, string searchTerm, int expectedListSize)
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetAll()).Returns(GetTestData());
            _mockUnitOfWork.Setup(uow => uow.Members).Returns(_mockRepo.Object);

            // Act
            var result = _controller.Index(searchBy, searchTerm);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<MemberViewModel>>(viewResult.ViewData.Model);
            Assert.Equal(expectedListSize, model.Count());
        }

        private IEnumerable<Member> GetTestData()
        {
            var members = new List<Member>()
            {
                new Member() { MemberId = 1, FirstName = "George", LastName = "Papas", PhoneNumber = "0000000000", MemberType = MemberType.Student },
                new Member() { MemberId = 1, FirstName = "George", LastName = "Papadopoulos", PhoneNumber = "0000000001", MemberType = MemberType.Student },
                new Member() { MemberId = 1, FirstName = "Maria", LastName = "Georgiou", PhoneNumber = "0000000002", MemberType = MemberType.Student }
            };

            return members;
        }
    }
}
