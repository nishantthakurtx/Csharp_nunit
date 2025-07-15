using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using CourseTech.API.Controllers;
using CourseTech.Core.DTOs.Course;
using CourseTech.Core.Services;
using CourseTech.Shared;
using CourseTech.Tests.Helpers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CourseTech.Tests.Controllers
{
    public class CoursesControllerTests
    {
        private readonly Mock<ICourseService> _courseServiceMock;
        private readonly CoursesController _controller;

        public CoursesControllerTests()
        {
            _courseServiceMock = new Mock<ICourseService>(MockBehavior.Strict);
            _controller = new CoursesController(_courseServiceMock.Object);
        }

        #region GetById

        [Fact]
        public async Task GetById_ReturnsOk_WhenCourseExists()
        {
            // Arrange
            var courseId = TestConstants.SampleCourseId;
            var dto = CourseTestData.GetSampleCourseDTO();

            CourseServiceMockHelper.SetupGetByIdSuccess(_courseServiceMock, courseId, dto);

            // Act
            var response = await _controller.GetById(courseId);

            // Assert

            var result = Assert.IsType<ObjectResult>(response);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(dto.Id, ((ServiceResult<CourseDTO>)result.Value!).Data!.Id);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenCourseNotFound()
        {
            // Arrange
            var courseId = TestConstants.SampleCourseId;
            CourseServiceMockHelper.SetupGetByIdFail(_courseServiceMock, courseId, TestConstants.ErrorMessageCourseNotFound);

            // Act

            var response = await _controller.GetById(courseId);

            // Assert

            var result = Assert.IsType<ObjectResult>(response);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal(TestConstants.ErrorMessageCourseNotFound, ((ServiceResult<CourseDTO>)result.Value!).ErrorMessage![0]);
        }

        #endregion

        #region GetAll

        [Fact]
        public async Task GetAll_ReturnsOk_WhenCoursesExist()
        {
            // Arrange

            var courses = CourseTestData.GetCourseDTOList();
            _courseServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(ServiceResult<IEnumerable<CourseDTO>>.Success(courses));

            // Act

            var response = await _controller.GetAll();

            // Assert

            var result = Assert.IsType<ObjectResult>(response);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(courses, ((ServiceResult<IEnumerable<CourseDTO>>)result.Value!).Data);
        }

        [Fact]
        public async Task GetAll_ReturnsOk_WhenNoCoursesExist()
        {
            // Arrange

            var empty = new List<CourseDTO>();
            _courseServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(ServiceResult<IEnumerable<CourseDTO>>.Success(empty));

            // Act

            var response = await _controller.GetAll();

            // Assert

            var result = Assert.IsType<ObjectResult>(response);
            Assert.Equal(200, result.StatusCode);
            Assert.Empty(((ServiceResult<IEnumerable<CourseDTO>>)result.Value!).Data!);
        }

        #endregion

        #region GetPublishedCourses

        [Fact]
        public async Task GetPublishedCourses_ReturnsOk_WhenCoursesExist()
        {
            var courses = CourseTestData.GetCourseDTOList();
            _courseServiceMock.Setup(s => s.GetPublishedCoursesAsync()).ReturnsAsync(ServiceResult<IEnumerable<CourseDTO>>.Success(courses));

            var response = await _controller.GetPublishedCourses();

            // Assert

            var result = Assert.IsType<ObjectResult>(response);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(courses, ((ServiceResult<IEnumerable<CourseDTO>>)result.Value!).Data);
        }

        [Fact]
        public async Task GetPublishedCourses_ReturnsOk_WhenEmptyList()
        {
            _courseServiceMock.Setup(s => s.GetPublishedCoursesAsync()).ReturnsAsync(ServiceResult<IEnumerable<CourseDTO>>.Success(new List<CourseDTO>()));

            var response = await _controller.GetPublishedCourses();

            // Assert

            var result = Assert.IsType<ObjectResult>(response);
            Assert.Equal(200, result.StatusCode);
            Assert.Empty(((ServiceResult<IEnumerable<CourseDTO>>)result.Value!).Data!);
        }

        #endregion

        #region GetCoursesByCategory

        [Fact]
        public async Task GetCoursesByCategory_ReturnsOk_WhenCoursesFound()
        {
            var categoryId = TestConstants.SampleCategoryId;
            var list = new List<CourseSummaryDTO>
            {
                new(Guid.NewGuid(), "Course 1", 100, "img.jpg", "Instructor", "Beginner")
            };

            _courseServiceMock.Setup(s => s.GetCoursesByCategoryAsync(categoryId)).ReturnsAsync(ServiceResult<IEnumerable<CourseSummaryDTO>>.Success(list));

            var result = await _controller.GetCoursesByCategory(categoryId);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, objectResult.StatusCode);
            Assert.Equal(list, ((ServiceResult<IEnumerable<CourseSummaryDTO>>)objectResult.Value!).Data);
        }

        [Fact]
        public async Task GetCoursesByCategory_ReturnsOk_WhenEmptyList()
        {
            var categoryId = TestConstants.SampleCategoryId;
            _courseServiceMock.Setup(s => s.GetCoursesByCategoryAsync(categoryId)).ReturnsAsync(ServiceResult<IEnumerable<CourseSummaryDTO>>.Success(new List<CourseSummaryDTO>()));

            var result = await _controller.GetCoursesByCategory(categoryId);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, objectResult.StatusCode);
            Assert.Empty(((ServiceResult<IEnumerable<CourseSummaryDTO>>)objectResult.Value!).Data!);
        }

        [Fact]
        public async Task GetCoursesByCategory_ReturnsBadRequest_WhenServiceFails()
        {
            var categoryId = TestConstants.SampleCategoryId;
            _courseServiceMock.Setup(s => s.GetCoursesByCategoryAsync(categoryId)).ReturnsAsync(ServiceResult<IEnumerable<CourseSummaryDTO>>.Fail(TestConstants.ErrorMessageCategoryNotFound));

            var result = await _controller.GetCoursesByCategory(categoryId);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, objectResult.StatusCode);
            Assert.Equal(TestConstants.ErrorMessageCategoryNotFound, ((ServiceResult<IEnumerable<CourseSummaryDTO>>)objectResult.Value!).ErrorMessage![0]);
        }

        #endregion

        #region GetCoursesByInstructor

        [Fact]
        public async Task GetCoursesByInstructor_ReturnsOk_WhenCoursesExist()
        {
            var instructorId = TestConstants.SampleInstructorId;
            var list = new List<CourseListDTO>
            {
                new(Guid.NewGuid(), "Course", "Desc", 500, "img.jpg", "Category", "Instructor", TimeSpan.FromHours(5), "Beginner", true)
            };

            _courseServiceMock.Setup(s => s.GetCoursesByInstructorAsync(instructorId)).ReturnsAsync(ServiceResult<IEnumerable<CourseListDTO>>.Success(list));

            var result = await _controller.GetCoursesByInstructor(instructorId);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, objectResult.StatusCode);
            Assert.Equal(list, ((ServiceResult<IEnumerable<CourseListDTO>>)objectResult.Value!).Data);
        }

        [Fact]
        public async Task GetCoursesByInstructor_ReturnsOk_WhenEmptyList()
        {
            var instructorId = TestConstants.SampleInstructorId;
            _courseServiceMock.Setup(s => s.GetCoursesByInstructorAsync(instructorId)).ReturnsAsync(ServiceResult<IEnumerable<CourseListDTO>>.Success(new List<CourseListDTO>()));

            var result = await _controller.GetCoursesByInstructor(instructorId);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, objectResult.StatusCode);
            Assert.Empty(((ServiceResult<IEnumerable<CourseListDTO>>)objectResult.Value!).Data!);
        }

        [Fact]
        public async Task GetCoursesByInstructor_ReturnsBadRequest_WhenServiceFails()
        {
            var instructorId = TestConstants.SampleInstructorId;
            _courseServiceMock.Setup(s => s.GetCoursesByInstructorAsync(instructorId)).ReturnsAsync(ServiceResult<IEnumerable<CourseListDTO>>.Fail(TestConstants.ErrorMessageInstructorNotFound));

            var result = await _controller.GetCoursesByInstructor(instructorId);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, objectResult.StatusCode);
            Assert.Equal(TestConstants.ErrorMessageInstructorNotFound, ((ServiceResult<IEnumerable<CourseListDTO>>)objectResult.Value!).ErrorMessage![0]);
        }

        #endregion

        #region GetCourseWithDetails

        [Fact]
        public async Task GetCourseWithDetails_ReturnsOk_WhenCourseExists()
        {
            var courseId = TestConstants.SampleCourseId;
            var courseDto = CourseTestData.GetSampleCourseDTO();

            _courseServiceMock
                .Setup(s => s.GetCourseWithDetailsAsync(courseId))
                .ReturnsAsync(ServiceResult<CourseDTO>.Success(courseDto));

            var response = await _controller.GetCourseWithDetails(courseId);

            var result = Assert.IsType<ObjectResult>(response);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(courseDto.Id, ((ServiceResult<CourseDTO>)result.Value!).Data!.Id);
        }

        [Fact]
        public async Task GetCourseWithDetails_ReturnsNotFound_WhenCourseDoesNotExist()
        {
            var courseId = TestConstants.SampleCourseId;

            _courseServiceMock
                .Setup(s => s.GetCourseWithDetailsAsync(courseId))
                .ReturnsAsync(ServiceResult<CourseDTO>.Fail(TestConstants.ErrorMessageCourseNotFound, HttpStatusCode.NotFound));

            var response = await _controller.GetCourseWithDetails(courseId);

            var result = Assert.IsType<ObjectResult>(response);
            Assert.Equal(404, result.StatusCode);
            Assert.Equal(TestConstants.ErrorMessageCourseNotFound, ((ServiceResult<CourseDTO>)result.Value!).ErrorMessage![0]);
        }

        [Fact]
        public async Task GetCourseWithDetails_ThrowsException_WhenServiceThrows()
        {
            var courseId = TestConstants.SampleCourseId;

            _courseServiceMock
                .Setup(s => s.GetCourseWithDetailsAsync(courseId))
                .ThrowsAsync(new Exception("DB Failure"));

            await Assert.ThrowsAsync<Exception>(() => _controller.GetCourseWithDetails(courseId));
        }

        #endregion

        #region GetAllCoursesSummariesForCard

        [Fact]
        public async Task GetAllCoursesSummariesForCard_ReturnsOk_WhenCoursesExist()
        {
            var courses = new List<CourseSummaryDTO>
    {
        new(Guid.NewGuid(), "Course 1", 150, "img.jpg", "Instructor", "Beginner")
    };

            _courseServiceMock
                .Setup(s => s.GetAllCoursesSummariesForCardAsync())
                .ReturnsAsync(ServiceResult<IEnumerable<CourseSummaryDTO>>.Success(courses));

            var response = await _controller.GetAllCoursesSummariesForCard();

            var result = Assert.IsType<ObjectResult>(response);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(courses, ((ServiceResult<IEnumerable<CourseSummaryDTO>>)result.Value!).Data);
        }

        [Fact]
        public async Task GetAllCoursesSummariesForCard_ReturnsOk_WhenEmptyList()
        {
            var empty = new List<CourseSummaryDTO>();

            _courseServiceMock
                .Setup(s => s.GetAllCoursesSummariesForCardAsync())
                .ReturnsAsync(ServiceResult<IEnumerable<CourseSummaryDTO>>.Success(empty));

            var response = await _controller.GetAllCoursesSummariesForCard();

            var result = Assert.IsType<ObjectResult>(response);
            Assert.Equal(200, result.StatusCode);
            Assert.Empty(((ServiceResult<IEnumerable<CourseSummaryDTO>>)result.Value!).Data!);
        }

        [Fact]
        public async Task GetAllCoursesSummariesForCard_ReturnsBadRequest_WhenServiceFails()
        {
            _courseServiceMock
                .Setup(s => s.GetAllCoursesSummariesForCardAsync())
                .ReturnsAsync(ServiceResult<IEnumerable<CourseSummaryDTO>>.Fail(TestConstants.ErrorMessageGeneric, HttpStatusCode.BadRequest));

            var response = await _controller.GetAllCoursesSummariesForCard();

            var result = Assert.IsType<ObjectResult>(response);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal(TestConstants.ErrorMessageGeneric, ((ServiceResult<IEnumerable<CourseSummaryDTO>>)result.Value!).ErrorMessage![0]);
        }

        [Fact]
        public async Task GetAllCoursesSummariesForCard_ThrowsException_WhenServiceThrows()
        {
            _courseServiceMock
                .Setup(s => s.GetAllCoursesSummariesForCardAsync())
                .ThrowsAsync(new Exception("Unexpected"));

            await Assert.ThrowsAsync<Exception>(() => _controller.GetAllCoursesSummariesForCard());
        }

        #endregion



        #region Create

        [Fact]
        public async Task Create_ReturnsOk_WhenSuccessful()
        {
            var createDto = CourseTestData.GetSampleCreateDTO();
            var dto = CourseTestData.GetSampleCourseDTO();

            _courseServiceMock.Setup(s => s.CreateAsync(createDto)).ReturnsAsync(ServiceResult<CourseDTO>.Success(dto));

            var result = await _controller.Create(createDto);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, objectResult.StatusCode);
            Assert.Equal(dto.Title, ((ServiceResult<CourseDTO>)objectResult.Value!).Data!.Title);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_WhenServiceFails()
        {
            var createDto = CourseTestData.GetSampleCreateDTO();
            _courseServiceMock.Setup(s => s.CreateAsync(createDto)).ReturnsAsync(ServiceResult<CourseDTO>.Fail(TestConstants.ErrorMessageInstructorNotFound));

            var result = await _controller.Create(createDto);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, objectResult.StatusCode);
            Assert.Equal(TestConstants.ErrorMessageInstructorNotFound, ((ServiceResult<CourseDTO>)objectResult.Value!).ErrorMessage![0]);
        }

        [Fact]
        public async Task Create_ThrowsException_WhenServiceThrows()
        {
            var createDto = CourseTestData.GetSampleCreateDTO();
            _courseServiceMock.Setup(s => s.CreateAsync(createDto)).ThrowsAsync(new Exception("Unexpected"));

            await Assert.ThrowsAsync<Exception>(() => _controller.Create(createDto));
        }

        #endregion

        #region Update

        [Fact]
        public async Task Update_ReturnsOk_WhenSuccessful()
        {
            // Arrange
            var updateDto = CourseTestData.GetSampleUpdateDTO();
            var updatedDto = CourseTestData.GetSampleCourseDTO();

            _courseServiceMock
                .Setup(s => s.UpdateAsync(updateDto))
                .ReturnsAsync(ServiceResult<CourseDTO>.Success(updatedDto));

            // Act
            var result = await _controller.Update(updateDto);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, objectResult.StatusCode);
            Assert.Equal(updatedDto.Title, ((ServiceResult<CourseDTO>)objectResult.Value!).Data!.Title);
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenServiceFails()
        {
            // Arrange
            var updateDto = CourseTestData.GetSampleUpdateDTO();

            _courseServiceMock
                .Setup(s => s.UpdateAsync(updateDto))
                .ReturnsAsync(ServiceResult<CourseDTO>.Fail(TestConstants.ErrorMessageGeneric, HttpStatusCode.BadRequest));

            // Act
            var result = await _controller.Update(updateDto);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, objectResult.StatusCode);
            Assert.Equal(TestConstants.ErrorMessageGeneric, ((ServiceResult<CourseDTO>)objectResult.Value!).ErrorMessage![0]);
        }

        [Fact]
        public async Task Update_ThrowsException_WhenServiceThrows()
        {
            // Arrange
            var updateDto = CourseTestData.GetSampleUpdateDTO();

            _courseServiceMock
                .Setup(s => s.UpdateAsync(updateDto))
                .ThrowsAsync(new Exception("Unexpected failure"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _controller.Update(updateDto));
        }

        #endregion

        #region SoftDelete

        [Fact]
        public async Task SoftDelete_ReturnsOk_WhenSuccessful()
        {
            var courseId = TestConstants.SampleCourseId;
            var result = ServiceResult.Success(HttpStatusCode.OK); // Use status code only

            _courseServiceMock.Setup(s => s.SoftDeleteAsync(courseId))
                .ReturnsAsync(result);

            var response = await _controller.SoftDelete(courseId);

            var objectResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal((int)HttpStatusCode.OK, objectResult.StatusCode);
            Assert.Equal(result, objectResult.Value);
        }

        [Fact]
        public async Task SoftDelete_ReturnsBadRequest_WhenCourseNotFound()
        {
            var courseId = TestConstants.SampleCourseId;
            var result = ServiceResult.Fail("Course not found", HttpStatusCode.BadRequest);

            _courseServiceMock.Setup(s => s.SoftDeleteAsync(courseId))
                .ReturnsAsync(result);

            var response = await _controller.SoftDelete(courseId);

            var objectResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, objectResult.StatusCode);
            Assert.Equal(result, objectResult.Value);
        }

        [Fact]
        public async Task SoftDelete_ThrowsException_WhenServiceThrows()
        {
            var courseId = TestConstants.SampleCourseId;
            _courseServiceMock.Setup(s => s.SoftDeleteAsync(courseId))
                .ThrowsAsync(new Exception("Unexpected error"));

            await Assert.ThrowsAsync<Exception>(() => _controller.SoftDelete(courseId));
        }

        #endregion

        #region PublishCourse

        [Fact]
        public async Task PublishCourse_ReturnsOk_WhenSuccessful()
        {
            var courseId = TestConstants.SampleCourseId;
            var result = ServiceResult.Success(HttpStatusCode.OK);

            _courseServiceMock.Setup(s => s.PublishCourseAsync(courseId))
                .ReturnsAsync(result);

            var response = await _controller.PublishCourse(courseId);

            var objectResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal((int)HttpStatusCode.OK, objectResult.StatusCode);
            Assert.Equal(result, objectResult.Value);
        }

        [Fact]
        public async Task PublishCourse_ReturnsBadRequest_WhenServiceFails()
        {
            var courseId = TestConstants.SampleCourseId;
            var result = ServiceResult.Fail("Course not found", HttpStatusCode.BadRequest);

            _courseServiceMock.Setup(s => s.PublishCourseAsync(courseId))
                .ReturnsAsync(result);

            var response = await _controller.PublishCourse(courseId);

            var objectResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, objectResult.StatusCode);
            Assert.Equal(result, objectResult.Value);
        }

        [Fact]
        public async Task PublishCourse_ThrowsException_WhenServiceThrows()
        {
            var courseId = TestConstants.SampleCourseId;

            _courseServiceMock.Setup(s => s.PublishCourseAsync(courseId))
                .ThrowsAsync(new Exception("Unexpected"));

            await Assert.ThrowsAsync<Exception>(() => _controller.PublishCourse(courseId));
        }

        #endregion

        #region UnpublishCourse

        [Fact]
        public async Task UnpublishCourse_ReturnsOk_WhenSuccessful()
        {
            var courseId = TestConstants.SampleCourseId;
            var result = ServiceResult.Success(HttpStatusCode.OK);

            _courseServiceMock.Setup(s => s.UnpublishCourseAsync(courseId))
                .ReturnsAsync(result);

            var response = await _controller.UnpublishCourse(courseId);

            var objectResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal((int)HttpStatusCode.OK, objectResult.StatusCode);
            Assert.Equal(result, objectResult.Value);
        }

        [Fact]
        public async Task UnpublishCourse_ReturnsBadRequest_WhenServiceFails()
        {
            var courseId = TestConstants.SampleCourseId;
            var result = ServiceResult.Fail("Course not found", HttpStatusCode.BadRequest);

            _courseServiceMock.Setup(s => s.UnpublishCourseAsync(courseId))
                .ReturnsAsync(result);

            var response = await _controller.UnpublishCourse(courseId);

            var objectResult = Assert.IsType<ObjectResult>(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, objectResult.StatusCode);
            Assert.Equal(result, objectResult.Value);
        }

        [Fact]
        public async Task UnpublishCourse_ThrowsException_WhenServiceThrows()
        {
            var courseId = TestConstants.SampleCourseId;

            _courseServiceMock.Setup(s => s.UnpublishCourseAsync(courseId))
                .ThrowsAsync(new Exception("Unexpected"));

            await Assert.ThrowsAsync<Exception>(() => _controller.UnpublishCourse(courseId));
        }

        #endregion



    }
}
