using CourseTech.Core.DTOs.Basket;
using CourseTech.Core.Services;
using CourseTech.Shared;
using Moq;
using System;

namespace CourseTech.Tests.Helpers
{
    public static class BasketServiceMockHelper
    {
        public static void SetupGetActiveBasketSuccess(Mock<IBasketService> mock, Guid userId, BasketDTO dto)
        {
            mock.Setup(s => s.GetActiveBasketAsync(userId))
                .ReturnsAsync(ServiceResult<BasketDTO>.Success(dto));
        }

        public static void SetupAddCourseFail(Mock<IBasketService> mock, Guid userId, Guid courseId, string errorMsg)
        {
            mock.Setup(s => s.AddCourseToBasketAsync(userId, courseId))
                .ReturnsAsync(ServiceResult.Fail(errorMsg));
        }
    }
}
