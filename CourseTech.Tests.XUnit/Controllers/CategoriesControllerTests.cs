using CourseTech.API.Controllers;
using CourseTech.Core.DTOs.Category;
using CourseTech.Core.Services;
using CourseTech.Shared;
using CourseTech.Tests.Helpers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace CourseTech.Tests.Controllers
{
    public class CategoriesControllerTests
    {
        private readonly Mock<ICategoryService> _categoryServiceMock;
        private readonly CategoriesController _controller;

        public CategoriesControllerTests()
        {
            _categoryServiceMock = new(MockBehavior.Strict);
            _controller = new CategoriesController(_categoryServiceMock.Object);
        }

        #region GetById

        [Fact]
        public async Task GetById_WithValidId_ReturnsOk()
        {
            var dto = CategoryTestData.GetSampleCategoryDTO();
            CategoryServiceMockHelper.SetupGetByIdSuccess(_categoryServiceMock, dto.Id, dto);

            var result = await _controller.GetById(dto.Id);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, objectResult.StatusCode);

            var actual = Assert.IsType<ServiceResult<CategoryDTO>>(objectResult.Value);
            Assert.True(actual.IsSuccess);
            Assert.Equal(dto.Id, actual.Data?.Id);
        }

        [Fact]
        public async Task GetById_WithInvalidId_ReturnsBadRequest()
        {
            var categoryId = TestConstants.SampleCategoryId;
            CategoryServiceMockHelper.SetupGetByIdFail(_categoryServiceMock, categoryId, TestConstants.ErrorMessageCategoryNotFound);

            var result = await _controller.GetById(categoryId);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, objectResult.StatusCode);

            var actual = Assert.IsType<ServiceResult<CategoryDTO>>(objectResult.Value);
            Assert.False(actual.IsSuccess);
            Assert.Equal(TestConstants.ErrorMessageCategoryNotFound, actual.ErrorMessage?[0]);
        }

        #endregion

        #region GetAll

        [Fact]
        public async Task GetAll_ReturnsOk()
        {
            var dtoList = CategoryTestData.GetSampleCategoryList();
            CategoryServiceMockHelper.SetupGetAllSuccess(_categoryServiceMock, dtoList);

            var result = await _controller.GetAll();

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, objectResult.StatusCode);

            var actual = Assert.IsType<ServiceResult<IEnumerable<CategoryDTO>>>(objectResult.Value);
            Assert.True(actual.IsSuccess);
        }

        #endregion

        #region GetCategoriesWithCourses

        [Fact]
        public async Task GetCategoriesWithCourses_ReturnsOk()
        {
            var dtoList = CategoryTestData.GetSampleCategoryList();
            CategoryServiceMockHelper.SetupGetCategoriesWithCoursesSuccess(_categoryServiceMock, dtoList);

            var result = await _controller.GetCategoriesWithCourses();

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, objectResult.StatusCode);

            var actual = Assert.IsType<ServiceResult<IEnumerable<CategoryDTO>>>(objectResult.Value);
            Assert.True(actual.IsSuccess);
        }

        #endregion

        #region GetCategoryWithCourses

        [Fact]
        public async Task GetCategoryWithCourses_WithValidId_ReturnsOk()
        {
            var dtoList = CategoryTestData.GetSampleCategoryList();
            CategoryServiceMockHelper.SetupGetCategoryWithCoursesSuccess(_categoryServiceMock, TestConstants.SampleCategoryId, dtoList);

            var result = await _controller.GetCategoryWithCourses(TestConstants.SampleCategoryId);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, objectResult.StatusCode);

            var actual = Assert.IsType<ServiceResult<IEnumerable<CategoryDTO>>>(objectResult.Value);
            Assert.True(actual.IsSuccess);
        }

        [Fact]
        public async Task GetCategoryWithCourses_WithInvalidId_ReturnsBadRequest()
        {
            CategoryServiceMockHelper.SetupGetCategoryWithCoursesFail(_categoryServiceMock, TestConstants.SampleCategoryId, TestConstants.ErrorMessageCategoryNotFound);

            var result = await _controller.GetCategoryWithCourses(TestConstants.SampleCategoryId);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, objectResult.StatusCode);

            var actual = Assert.IsType<ServiceResult<IEnumerable<CategoryDTO>>>(objectResult.Value);
            Assert.False(actual.IsSuccess);
            Assert.Equal(TestConstants.ErrorMessageCategoryNotFound, actual.ErrorMessage?[0]);
        }

        #endregion

        #region Create

        [Fact]
        public async Task Create_WhenSuccessful_ReturnsCreated()
        {
            var createDto = new CategoryCreateDTO("Test Category");
            var resultDto = new CategoryDTO(TestConstants.SampleCategoryId, createDto.Name);

            CategoryServiceMockHelper.SetupCreateSuccess(_categoryServiceMock, createDto, resultDto);

            var result = await _controller.Create(createDto);

            var createdResult = Assert.IsType<CreatedResult>(result);
            Assert.Equal((int)HttpStatusCode.Created, createdResult.StatusCode);

            var actual = Assert.IsType<CategoryDTO>(createdResult.Value);
            Assert.Equal(resultDto.Id, actual.Id);
            Assert.Equal(resultDto.Name, actual.Name);
        }



        #endregion

        #region Update

        [Fact]
        public async Task Update_WhenSuccessful_ReturnsOk()
        {
            var updateDto = new CategoryUpdateDTO(TestConstants.SampleCategoryId, "Updated Name");

            CategoryServiceMockHelper.SetupUpdateSuccess(_categoryServiceMock, updateDto);

            var result = await _controller.Update(updateDto);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, objectResult.StatusCode);

            var actual = Assert.IsType<ServiceResult<CategoryDTO>>(objectResult.Value);
            Assert.True(actual.IsSuccess);
        }

        #endregion

        #region Delete

        [Fact]
        public async Task Delete_WhenSuccessful_ReturnsOk()
        {
            CategoryServiceMockHelper.SetupDeleteSuccess(_categoryServiceMock, TestConstants.SampleCategoryId);

            var result = await _controller.Delete(TestConstants.SampleCategoryId);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, objectResult.StatusCode);

            var actual = Assert.IsType<ServiceResult>(objectResult.Value);
            Assert.True(actual.IsSuccess);
        }

        [Fact]
        public async Task Delete_WhenFailed_ReturnsBadRequest()
        {
            CategoryServiceMockHelper.SetupDeleteFail(_categoryServiceMock, TestConstants.SampleCategoryId, TestConstants.ErrorMessageCategoryNotFound);

            var result = await _controller.Delete(TestConstants.SampleCategoryId);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, objectResult.StatusCode);

            var actual = Assert.IsType<ServiceResult>(objectResult.Value);
            Assert.False(actual.IsSuccess);
            Assert.Equal(TestConstants.ErrorMessageCategoryNotFound, actual.ErrorMessage?[0]);
        }

        #endregion
    }
}
