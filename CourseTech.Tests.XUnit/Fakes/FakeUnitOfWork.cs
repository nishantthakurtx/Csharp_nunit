using CourseTech.Core.Models;
using CourseTech.Core.Repositories;
using CourseTech.Core.Services;
using CourseTech.Core.UnitOfWorks;

namespace CourseTech.Tests.Fakes
{
    public class FakeUnitOfWork : IUnitOfWork
    {
        public FakeUnitOfWork()
        {
            Course = new FakeCourseRepository();
            Category = new FakeCategoryRepository();
        }


        public FakeCourseRepository Course { get; private set; } = new();
        public FakeCategoryRepository Category { get; private set; } = new();

        public IEnrollmentRepository Enrollment => throw new NotImplementedException();
        public IBasketRepository Basket => throw new NotImplementedException();
        public IAppUserRefreshTokenRepository AppUserRefreshToken => throw new NotImplementedException();
        public IPaymentRepository Payment => throw new NotImplementedException();
        public IOrderRepository Order => throw new NotImplementedException();

        ICategoryRepository IUnitOfWork.Category => Category;

        ICourseRepository IUnitOfWork.Course => Course;

        public Task SaveChangesAsync() => Task.CompletedTask;
        public void SaveChanges() { }

        // Helper for test
        public void SetFakeCategory(Category category) => Category.SetFakeCategory(category);

    }
}
