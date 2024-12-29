using AutoMapper;
using CourseTech.Core.DTOs.AppUser;
using CourseTech.Core.DTOs.Course;
using CourseTech.Core.Models;
using CourseTech.Core.Services;
using CourseTech.Core.UnitOfWorks;
using CourseTech.Shared;

namespace CourseTech.Service.Services
{
    public class EnrollmentService(IUnitOfWork unitOfWork, IMapper mapper) : IEnrollmentService
    {
        public async Task<ServiceResult> EnrollAsync(Guid userId, Guid courseId)
        {
            var existingEnrollment = await unitOfWork.Enrollment.GetEnrollmentAsync(userId, courseId);
            if (existingEnrollment != null)
                return ServiceResult.Fail("User already enrolled in this course.");

            var enrollment = new Enrollment { AppUserId = userId, CourseId = courseId };
            await unitOfWork.Enrollment.InsertAsync(enrollment);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success();
        }

        public async Task<ServiceResult<IEnumerable<CourseDTO>>> GetEnrolledCoursesByUserAsync(Guid userId)
        {
            var enrollments = await unitOfWork.Enrollment.GetEnrollmentsByUserAsync(userId);
            var courses = mapper.Map<IEnumerable<CourseDTO>>(enrollments.Select(e => e.Course));

            return ServiceResult<IEnumerable<CourseDTO>>.Success(courses);
        }


        public async Task<ServiceResult<IEnumerable<AppUserDTO>>> GetEnrolledUsersByCourseAsync(Guid courseId)
        {
            var enrollments = await unitOfWork.Enrollment.GetEnrollmentsByCourseAsync(courseId);
            var users = mapper.Map<IEnumerable<AppUserDTO>>(enrollments.Select(e => e.AppUser));

            return ServiceResult<IEnumerable<AppUserDTO>>.Success(users);
        }


        public async Task<ServiceResult> UnenrollAsync(Guid userId, Guid courseId)
        {
            var existingEnrollment = await unitOfWork.Enrollment.GetEnrollmentAsync(userId, courseId);
            if (existingEnrollment == null)
            {
                return ServiceResult.Fail("Enrollment not found.");
            }

            existingEnrollment.MarkAsDeleted();
            unitOfWork.Enrollment.Update(existingEnrollment);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success();
        }
    }
}