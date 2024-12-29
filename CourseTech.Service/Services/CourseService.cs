using AutoMapper;
using CourseTech.Core.DTOs.Course;
using CourseTech.Core.Models;
using CourseTech.Core.Services;
using CourseTech.Core.UnitOfWorks;
using CourseTech.Shared;

namespace CourseTech.Service.Services
{
    public class CourseService(IUnitOfWork unitOfWork, IMapper mapper) : ICourseService
    {
        public async Task<ServiceResult<CourseDTO>> GetByIdAsync(Guid courseId)
        {
            var entity = await unitOfWork.Course.GetByIdAsync(courseId);
            if (entity == null)
                return ServiceResult<CourseDTO>.Fail($"Course ({courseId}) not found.");

            var courseDto = mapper.Map<CourseDTO>(entity);

            return ServiceResult<CourseDTO>.Success(courseDto);
        }

        public async Task<ServiceResult<IEnumerable<CourseDTO>>> GetAllAsync()
        {
            var entities = await unitOfWork.Course.GetAllCoursesWithDetailsAsync();
            var courseDtos = mapper.Map<IEnumerable<CourseDTO>>(entities);
            return ServiceResult<IEnumerable<CourseDTO>>.Success(courseDtos);
        }

        public async Task<ServiceResult<IEnumerable<CourseDTO>>> GetPublishedCoursesAsync()
        {
            var entities = await unitOfWork.Course.GetPublishedCoursesAsync();
            var courseDtos = mapper.Map<IEnumerable<CourseDTO>>(entities);

            return ServiceResult<IEnumerable<CourseDTO>>.Success(courseDtos);
        }

        public async Task<ServiceResult<IEnumerable<CourseSummaryDTO>>> GetAllCoursesSummariesForCardAsync()
        {
            var entities = await unitOfWork.Course.GetPublishedCoursesAsync();
            var courseSummaries = mapper.Map<IEnumerable<CourseSummaryDTO>>(entities);

            return ServiceResult<IEnumerable<CourseSummaryDTO>>.Success(courseSummaries);
        }

        public async Task<ServiceResult<IEnumerable<CourseDTO>>> GetCoursesByCategoryAsync(Guid categoryId)
        {
            var entities = await unitOfWork.Course.GetCoursesByCategoryAsync(categoryId);
            var courseDtos = mapper.Map<IEnumerable<CourseDTO>>(entities);

            return ServiceResult<IEnumerable<CourseDTO>>.Success(courseDtos);
        }

        public async Task<ServiceResult<IEnumerable<CourseDTO>>> GetCoursesByInstructorAsync(Guid instructorId)
        {
            var entities = await unitOfWork.Course.GetCoursesByInstructorAsync(instructorId);
            var courseDtos = mapper.Map<IEnumerable<CourseDTO>>(entities);

            return ServiceResult<IEnumerable<CourseDTO>>.Success(courseDtos);
        }

        public async Task<ServiceResult<CourseDTO>> GetCourseWithDetailsAsync(Guid courseId)
        {
            var entity = await unitOfWork.Course.GetCourseWithDetailsAsync(courseId);
            if (entity == null)
                return ServiceResult<CourseDTO>.Fail($"Course ({courseId}) not found.");

            var courseDto = mapper.Map<CourseDTO>(entity);

            return ServiceResult<CourseDTO>.Success(courseDto);
        }


        public async Task<ServiceResult<CourseDTO>> CreateAsync(CourseCreateDTO courseDto)
        {
            var entity = mapper.Map<Course>(courseDto);

            await unitOfWork.Course.InsertAsync(entity);
            await unitOfWork.SaveChangesAsync();

            var newCourseDto = mapper.Map<CourseDTO>(entity);
            return ServiceResult<CourseDTO>.Success(newCourseDto);
        }

        public async Task<ServiceResult<CourseDTO>> UpdateAsync(CourseUpdateDTO courseDto)
        {
            var existingEntity = await unitOfWork.Course.GetByIdAsync(courseDto.Id);
            if (existingEntity == null)
                return ServiceResult<CourseDTO>.Fail($"Course ({courseDto.Id}) not found.");

            mapper.Map(courseDto, existingEntity);
            unitOfWork.Course.Update(existingEntity);
            await unitOfWork.SaveChangesAsync();

            var updatedCourseDto = mapper.Map<CourseDTO>(existingEntity);
            return ServiceResult<CourseDTO>.Success(updatedCourseDto);
        }

        public async Task<ServiceResult> SoftDeleteAsync(Guid courseId)
        {
            var existingEntity = await unitOfWork.Course.GetByIdAsync(courseId);
            if (existingEntity == null)
                return ServiceResult.Fail($"Course ({courseId}) not found.");

            existingEntity.MarkAsDeleted();
            existingEntity.Unpublish();
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success();
        }

        public async Task<ServiceResult> PublishCourseAsync(Guid courseId)
        {
            var existingEntity = await unitOfWork.Course.GetByIdAsync(courseId);
            if (existingEntity == null)
                return ServiceResult.Fail($"Course ({courseId}) not found.");

            existingEntity.Publish();
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success();
        }

        public async Task<ServiceResult> UnpublishCourseAsync(Guid courseId)
        {
            var existingEntity = await unitOfWork.Course.GetByIdAsync(courseId);
            if (existingEntity == null)
                return ServiceResult.Fail($"Course ({courseId}) not found.");

            existingEntity.Unpublish();
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success();
        }
    }
}