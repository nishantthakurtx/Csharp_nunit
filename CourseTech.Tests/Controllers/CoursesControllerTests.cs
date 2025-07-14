using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CourseTech.API.Controllers;
using CourseTech.Core.DTOs.Course;
using CourseTech.Core.Services;
using CourseTech.Shared;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Net;

namespace CourseTech.Tests.Controllers
{
    [TestFixture]
    public class CoursesControllerTests
    {
        private Mock<ICourseService> _courseServiceMock;
        private CoursesController _controller;

        [SetUp]
        public void SetUp()
        {
            _courseServiceMock = new Mock<ICourseService>();
            _controller = new CoursesController(_courseServiceMock.Object);
        }

        #region  CoursesGetById

        [Test]
        public async Task GetById_ReturnsOk_WhenCourseExists()
        {
            var courseId = Guid.NewGuid();
            var dto = new CourseDTO(courseId, "Test Course", "Desc", "img", "vid", "Beginner", "English", 100, TimeSpan.FromHours(5), DateTime.Now, "Instructor", "Category", DateTime.Now);
            var result = ServiceResult<CourseDTO>.Success(dto);

            _courseServiceMock.Setup(s => s.GetByIdAsync(courseId)).ReturnsAsync(result);

            var response = await _controller.GetById(courseId);

            Assert.IsInstanceOf<ObjectResult>(response);
            var objResult = response as ObjectResult;
            Assert.AreEqual(200, objResult.StatusCode);
            Assert.AreEqual(result, objResult.Value);
        }

        [Test]
        public async Task GetById_ReturnsNotFound_WhenCourseNotFound()
        {
            var courseId = Guid.NewGuid();
            var result = ServiceResult<CourseDTO>.Fail("Course not found", HttpStatusCode.NotFound);

            _courseServiceMock.Setup(s => s.GetByIdAsync(courseId)).ReturnsAsync(result);

            var response = await _controller.GetById(courseId);

            Assert.IsInstanceOf<ObjectResult>(response);
            var objResult = response as ObjectResult;
            Assert.AreEqual(404, objResult.StatusCode);
            Assert.AreEqual(result, objResult.Value);
        }
#endregion

        #region  GetAllCourses

        [Test]
        public async Task GetAll_ReturnsOk_WhenCoursesExist()
        {
            var courses = new List<CourseDTO>
            {
                new CourseDTO(Guid.NewGuid(), "Title", "Desc", "img", "vid", "Intermediate", "English", 150, TimeSpan.FromHours(3), DateTime.Now, "Instructor", "Category", DateTime.Now)
            };
            var result = ServiceResult<IEnumerable<CourseDTO>>.Success(courses);

            _courseServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(result);

            var response = await _controller.GetAll();

            Assert.IsInstanceOf<ObjectResult>(response);
            var objResult = response as ObjectResult;
            Assert.AreEqual(200, objResult.StatusCode);
            Assert.AreEqual(result, objResult.Value);
        }

        [Test]
        public async Task GetAll_ReturnsOk_WhenNoCoursesExist()
        {
            var result = ServiceResult<IEnumerable<CourseDTO>>.Success(new List<CourseDTO>());

            _courseServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(result);

            var response = await _controller.GetAll();

            Assert.IsInstanceOf<ObjectResult>(response);
            var objResult = response as ObjectResult;
            Assert.AreEqual(200, objResult.StatusCode);
            Assert.IsEmpty((IEnumerable<CourseDTO>)result.Data);
        }

        //[Test]
        //public async Task GetAll_ReturnsError_WhenServiceFails()
        //{
        //    //Arrange
        //    var result = ServiceResult<IEnumerable<CourseDTO>>.Fail("Failed to retrieve", HttpStatusCode.BadRequest);

        //    _courseServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(result);

        //    //Act 

        //    var response = await _controller.GetAll();

        //    //Assert

        //    Assert.IsInstanceOf<ObjectResult>(response);
        //    var objResult = response as ObjectResult;
        //    Assert.AreEqual(400, objResult.StatusCode);
        //    Assert.AreEqual(result, objResult.Value);
        //}
        #endregion

        #region GetPublishedCourses

        [Test]
        public async Task GetPublishedCourses_ReturnsOk_WhenCoursesExist()
        {
            var courses = new List<CourseDTO>
            {
                new CourseDTO(Guid.NewGuid(), "Published Course", "Desc", "img", "vid", "Advanced", "English", 200, TimeSpan.FromHours(4), DateTime.Now, "Instructor", "Category", DateTime.Now)
            };
            var result = ServiceResult<IEnumerable<CourseDTO>>.Success(courses);

            _courseServiceMock.Setup(s => s.GetPublishedCoursesAsync()).ReturnsAsync(result);

            var response = await _controller.GetPublishedCourses();

            Assert.IsInstanceOf<ObjectResult>(response);
            var objResult = response as ObjectResult;
            Assert.AreEqual(200, objResult.StatusCode);
            Assert.AreEqual(result, objResult.Value);
        }

        [Test]
        public async Task GetPublishedCourses_ReturnsOk_WhenNoCoursesExist()
        {
            //arrange

            var result = ServiceResult<IEnumerable<CourseDTO>>.Success(new List<CourseDTO>());

            _courseServiceMock.Setup(s => s.GetPublishedCoursesAsync()).ReturnsAsync(result);

            //act

            var response = await _controller.GetPublishedCourses();

            //assert

            Assert.IsInstanceOf<ObjectResult>(response);
            var objResult = response as ObjectResult;
            Assert.AreEqual(200, objResult.StatusCode);
            Assert.IsEmpty((IEnumerable<CourseDTO>)result.Data);
        }

        //[Test]
        //public async Task GetPublishedCourses_ReturnsError_WhenServiceFails()
        //{
        //    var result = ServiceResult<IEnumerable<CourseDTO>>.Fail("Service error", HttpStatusCode.InternalServerError);

        //    _courseServiceMock.Setup(s => s.GetPublishedCoursesAsync()).ReturnsAsync(result);

        //    var response = await _controller.GetPublishedCourses();

        //    Assert.IsInstanceOf<ObjectResult>(response);
        //    var objResult = response as ObjectResult;
        //    Assert.AreEqual(500, objResult.StatusCode);
        //    Assert.AreEqual(result, objResult.Value);
        //}
        #endregion

        #region GetCoursesByCategory
        [Test]
        public async Task GetCoursesByCategory_ReturnsOk_WhenCoursesFound()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var courses = new List<CourseSummaryDTO>
            {
                new(Guid.NewGuid(), "Course 1", 100, "img.jpg", "Instructor", "Beginner")
            };
            var serviceResult = ServiceResult<IEnumerable<CourseSummaryDTO>>.Success(courses);

            _courseServiceMock
                .Setup(s => s.GetCoursesByCategoryAsync(categoryId))
                .ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.GetCoursesByCategory(categoryId);

            // Assert
            var okResult = result as ObjectResult;
            Assert.IsInstanceOf<ObjectResult>(result);
            Assert.AreEqual((int)HttpStatusCode.OK, okResult!.StatusCode);
            Assert.AreEqual(serviceResult, okResult.Value);
        }

        [Test]
        public async Task GetCoursesByCategory_ReturnsOk_WhenEmptyList()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var serviceResult = ServiceResult<IEnumerable<CourseSummaryDTO>>.Success(new List<CourseSummaryDTO>());

            _courseServiceMock
                .Setup(s => s.GetCoursesByCategoryAsync(categoryId))
                .ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.GetCoursesByCategory(categoryId);

            // Assert
            var okResult = result as ObjectResult;
            Assert.IsInstanceOf<ObjectResult>(result);
            Assert.AreEqual((int)HttpStatusCode.OK, okResult!.StatusCode);
            Assert.AreEqual(serviceResult, okResult.Value);
        }

        [Test]
        public async Task GetCoursesByCategory_ReturnsBadRequest_WhenServiceFails()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var serviceResult = ServiceResult<IEnumerable<CourseSummaryDTO>>.Fail("Category not found", HttpStatusCode.BadRequest);

            _courseServiceMock
                .Setup(s => s.GetCoursesByCategoryAsync(categoryId))
                .ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.GetCoursesByCategory(categoryId);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.IsInstanceOf<ObjectResult>(result);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, objectResult!.StatusCode);
            Assert.AreEqual(serviceResult, objectResult.Value);
        }
        #endregion
    }
}
