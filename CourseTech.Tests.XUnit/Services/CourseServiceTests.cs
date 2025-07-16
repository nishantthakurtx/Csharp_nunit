using AutoMapper;
using CourseTech.Core.DTOs.Course;
using CourseTech.Core.Models;
using CourseTech.Service.Services;
using CourseTech.Shared;
using CourseTech.Tests.Fakes;
using CourseTech.Tests.Helpers;
using CourseTech.Tests.Helpers.Mapping;
using CourseTech.Tests.Mocks;


namespace CourseTech.Tests.Services
{
    public class CourseServiceTests
    {
        private readonly CourseService _service;
        private readonly FakeUnitOfWork _unitOfWork;
        private readonly MockUserManager _userManager;
        private readonly IMapper _mapper;

        public CourseServiceTests()
        {
            _unitOfWork = new FakeUnitOfWork();
            _userManager = new MockUserManager();
            _mapper = TestMapper.CreateMapper(); 
            _service = new CourseService(_unitOfWork, _userManager.Object, _mapper);
        }

        #region GetByIdAsync

        [Fact]
        public async Task GetByIdAsync_ReturnsSuccess_WhenFound()
        {
            var id = Guid.NewGuid();
            _unitOfWork.Course.InsertFake(new Course("Test", "desc", "", "", 0, 0, 0, TimeSpan.Zero, Guid.Empty, Guid.Empty) { Id = id });

            var result = await _service.GetByIdAsync(id);

            Assert.True(result.IsSuccess);
            Assert.Equal(id, result.Data!.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsFail_WhenNotFound()
        {
            var result = await _service.GetByIdAsync(Guid.NewGuid());

            Assert.True(result.IsFailure);
            Assert.Equal("Course", result.ErrorMessage![0].Split(' ')[0]);
        }

        #endregion

        #region GetAllAsync

        [Fact]
        public async Task GetAllAsync_ReturnsSuccess()
        {
            _unitOfWork.Course.InsertFake(new Course("Test", "desc", "", "", 0, 0, 0, TimeSpan.Zero, Guid.Empty, Guid.Empty));
            var result = await _service.GetAllAsync();

            Assert.True(result.IsSuccess);
            Assert.NotEmpty(result.Data!);
        }

        #endregion

        #region GetPublishedCoursesAsync

        [Fact]
        public async Task GetPublishedCoursesAsync_ReturnsPublishedCourses()
        {
            var course = new Course("Pub", "desc", "", "", 0, 0, 0, TimeSpan.Zero, Guid.Empty, Guid.Empty);
            course.Publish();
            _unitOfWork.Course.InsertFake(course);

            var result = await _service.GetPublishedCoursesAsync();

            Assert.True(result.IsSuccess);
            Assert.True(result.Data!.All(c => c.PublishedAt != null));
        }

        #endregion

        #region GetAllCoursesSummariesForCardAsync

        [Fact]
        public async Task GetAllCoursesSummariesForCardAsync_ReturnsSuccess()
        {
            var course = new Course("Card", "desc", "", "", 0, 0, 0, TimeSpan.Zero, Guid.Empty, Guid.Empty);
            course.Publish();
            _unitOfWork.Course.InsertFake(course);

            var result = await _service.GetAllCoursesSummariesForCardAsync();

            Assert.True(result.IsSuccess);
        }

        #endregion

        #region GetCoursesByCategoryAsync

        [Fact]
        public async Task GetCoursesByCategoryAsync_ReturnsSuccess()
        {
            var categoryId = Guid.NewGuid();
            var course = new Course("Cat", "desc", "", "", 0, 0, 0, TimeSpan.Zero, Guid.Empty, categoryId);
            _unitOfWork.Course.InsertFake(course);

            var result = await _service.GetCoursesByCategoryAsync(categoryId);

            Assert.True(result.IsSuccess);
        }

        #endregion

        #region GetCoursesByInstructorAsync

        [Fact]
        public async Task GetCoursesByInstructorAsync_ReturnsSuccess()
        {
            var instructorId = Guid.NewGuid();
            var course = new Course("Inst", "desc", "", "", 0, 0, 0, TimeSpan.Zero, instructorId, Guid.NewGuid());
            _unitOfWork.Course.InsertFake(course);

            var result = await _service.GetCoursesByInstructorAsync(instructorId);

            Assert.True(result.IsSuccess);
        }

        #endregion

        #region GetCourseWithDetailsAsync

        [Fact]
        public async Task GetCourseWithDetailsAsync_ReturnsSuccess_WhenFound()
        {
            var id = Guid.NewGuid();
            _unitOfWork.Course.InsertFake(new Course("Details", "desc", "", "", 0, 0, 0, TimeSpan.Zero, Guid.Empty, Guid.Empty) { Id = id });

            var result = await _service.GetCourseWithDetailsAsync(id);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task GetCourseWithDetailsAsync_ReturnsFail_WhenNotFound()
        {
            var result = await _service.GetCourseWithDetailsAsync(Guid.NewGuid());

            Assert.True(result.IsFailure);
        }

        #endregion

        #region CreateAsync

        [Fact]
        public async Task CreateAsync_ReturnsSuccess()
        {
            var dto = CourseTestData.GetSampleCreateDTO();

            var fakeCategory = new Category
            {
                Id = TestConstants.SampleCategoryId,
                Name = "Programming"
            };

            var result = await _service.CreateAsync(dto);

            Assert.True(result.IsSuccess);
            Assert.Equal(dto.Title, result.Data!.Title);
        }

        #endregion

        #region UpdateAsync

        [Fact]
        public async Task UpdateAsync_ReturnsSuccess()
        {
            var id = Guid.NewGuid();
            var course = new Course("Old", "desc", "", "", 0, 0, 0, TimeSpan.Zero, Guid.NewGuid(), Guid.NewGuid()) { Id = id };
            _unitOfWork.Course.InsertFake(course);


            var dto = CourseTestData.GetSampleUpdateDTO() with { Id = id };
            var result = await _service.UpdateAsync(dto);

            Assert.True(result.IsSuccess);
            Assert.Equal(dto.Title, result.Data!.Title);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsFail_WhenNotFound()
        {
            var dto = CourseTestData.GetSampleUpdateDTO();

            var result = await _service.UpdateAsync(dto);

            Assert.True(result.IsFailure);
        }

        #endregion

        #region SoftDeleteAsync

        [Fact]
        public async Task SoftDeleteAsync_ReturnsSuccess()
        {
            var id = Guid.NewGuid();
            _unitOfWork.Course.InsertFake(new Course("Del", "desc", "", "", 0, 0, 0, TimeSpan.Zero, Guid.Empty, Guid.Empty) { Id = id });

            var result = await _service.SoftDeleteAsync(id);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task SoftDeleteAsync_ReturnsFail_WhenNotFound()
        {
            var result = await _service.SoftDeleteAsync(Guid.NewGuid());

            Assert.True(result.IsFail);
        }

        #endregion

        #region PublishCourseAsync

        [Fact]
        public async Task PublishCourseAsync_ReturnsSuccess()
        {
            var id = Guid.NewGuid();
            _unitOfWork.Course.InsertFake(new Course("Pub", "desc", "", "", 0, 0, 0, TimeSpan.Zero, Guid.Empty, Guid.Empty) { Id = id });

            var result = await _service.PublishCourseAsync(id);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task PublishCourseAsync_ReturnsFail_WhenNotFound()
        {
            var result = await _service.PublishCourseAsync(Guid.NewGuid());

            Assert.True(result.IsFail);
        }

        #endregion

        #region UnpublishCourseAsync

        [Fact]
        public async Task UnpublishCourseAsync_ReturnsSuccess()
        {
            var id = Guid.NewGuid();
            var course = new Course("Unpub", "desc", "", "", 0, 0, 0, TimeSpan.Zero, Guid.Empty, Guid.Empty) { Id = id };
            course.Publish();
            _unitOfWork.Course.InsertFake(course);

            var result = await _service.UnpublishCourseAsync(id);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task UnpublishCourseAsync_ReturnsFail_WhenNotFound()
        {
            var result = await _service.UnpublishCourseAsync(Guid.NewGuid());

            Assert.True(result.IsFail);
        }

        #endregion

        [Fact]
        public void Where_ReturnsFilteredCourses()
        {
            var instructorId = Guid.NewGuid();
            _unitOfWork.Course.InsertFake(new Course("C1", "desc", "", "", 0, 0, 0, TimeSpan.Zero, instructorId, Guid.NewGuid()));
            _unitOfWork.Course.InsertFake(new Course("C2", "desc", "", "", 0, 0, 0, TimeSpan.Zero, Guid.NewGuid(), Guid.NewGuid()));

            var filtered = _unitOfWork.Course.Where(c => c.InstructorId == instructorId).ToList();

            Assert.Single(filtered);
            Assert.Equal("C1", filtered[0].Title);
        }
    }
}
