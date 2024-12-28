using CourseTech.Core.Repositories;
using CourseTech.Core.UnitOfWorks;
using CourseTech.Repository.Repositories;

namespace CourseTech.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public ICategoryRepository Category { get; private set; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Category = new CategoryRepository(context);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}