using CourseTech.Core.DTOs.Course;
using CourseTech.Core.Services;
using CourseTech.Shared;
using Moq;

namespace CourseTech.Tests.Helpers
{
    public static class CourseServiceMockHelper
    {
        public static void SetupGetByIdSuccess(Mock<ICourseService> serviceMock, Guid courseId, CourseDTO dto)
        {
            serviceMock.Setup(s => s.GetByIdAsync(courseId)).ReturnsAsync(ServiceResult<CourseDTO>.Success(dto));
        }

        public static void SetupGetByIdFail(Mock<ICourseService> serviceMock, Guid courseId, string error)
        {
            serviceMock.Setup(s => s.GetByIdAsync(courseId)).ReturnsAsync(ServiceResult<CourseDTO>.Fail(error));
        }
    }
}
