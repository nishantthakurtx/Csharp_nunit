using CourseTech.Core.DTOs.Category;
using CourseTech.Core.Services;
using CourseTech.Shared;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;

namespace CourseTech.Tests.Helpers
{
    public static class CategoryServiceMockHelper
    {
        public static void SetupGetByIdSuccess(Mock<ICategoryService> mock, Guid id, CategoryDTO dto)
        {
            mock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(ServiceResult<CategoryDTO>.Success(dto));
        }

        public static void SetupGetByIdFail(Mock<ICategoryService> mock, Guid id, string error)
        {
            mock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(ServiceResult<CategoryDTO>.Fail(error));
        }

        public static void SetupGetAllSuccess(Mock<ICategoryService> mock, IEnumerable<CategoryDTO> data)
        {
            mock.Setup(s => s.GetAllAsync()).ReturnsAsync(ServiceResult<IEnumerable<CategoryDTO>>.Success(data));
        }

        public static void SetupGetCategoriesWithCoursesSuccess(Mock<ICategoryService> mock, IEnumerable<CategoryDTO> data)
        {
            mock.Setup(s => s.GetCategoriesWithCoursesAsync()).ReturnsAsync(ServiceResult<IEnumerable<CategoryDTO>>.Success(data));
        }

        public static void SetupGetCategoryWithCoursesSuccess(Mock<ICategoryService> mock, Guid id, IEnumerable<CategoryDTO> data)
        {
            mock.Setup(s => s.GetCategoryWithCoursesAsync(id)).ReturnsAsync(ServiceResult<IEnumerable<CategoryDTO>>.Success(data));
        }

        public static void SetupGetCategoryWithCoursesFail(Mock<ICategoryService> mock, Guid id, string error)
        {
            mock.Setup(s => s.GetCategoryWithCoursesAsync(id)).ReturnsAsync(ServiceResult<IEnumerable<CategoryDTO>>.Fail(error));
        }

        public static void SetupCreateSuccess(Mock<ICategoryService> mock, CategoryCreateDTO input, CategoryDTO output)
        {
            mock.Setup(s => s.CreateAsync(input)).ReturnsAsync(ServiceResult<CategoryDTO>.SuccessAsCreated(output, $"api/categories/{output.Id}"));
        }

        public static void SetupUpdateSuccess(Mock<ICategoryService> mock, CategoryUpdateDTO dto)
        {
            mock.Setup(s => s.UpdateAsync(dto)).ReturnsAsync(ServiceResult<CategoryDTO>.Success(new CategoryDTO(dto.Id, dto.Name)));
        }

        public static void SetupDeleteSuccess(Mock<ICategoryService> mock, Guid id)
        {
            mock.Setup(s => s.SoftDeleteAsync(id)).ReturnsAsync(ServiceResult.Success());
        }

        public static void SetupDeleteFail(Mock<ICategoryService> mock, Guid id, string error)
        {
            mock.Setup(s => s.SoftDeleteAsync(id)).ReturnsAsync(ServiceResult.Fail(error));
        }
    }
}
