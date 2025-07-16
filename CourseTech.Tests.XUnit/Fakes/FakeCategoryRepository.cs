using CourseTech.Core.Models;
using CourseTech.Core.Repositories;
using System.Linq.Expressions;

public class FakeCategoryRepository : ICategoryRepository
{
    public List<Category> Categories = new();

    public Task<Category?> GetByIdAsync(Guid id) =>
        Task.FromResult(Categories.FirstOrDefault(c => c.Id == id));

    public Task<IEnumerable<Category>> GetCategoriesWithCoursesAsync() =>
        Task.FromResult(Categories.AsEnumerable());

    public Task<IEnumerable<Category>> GetCategoryWithCoursesAsync(Guid id) =>
        Task.FromResult(Categories.Where(c => c.Id == id).AsEnumerable());

    public Task<IEnumerable<Category>> GetAllAsync() =>
        Task.FromResult(Categories.AsEnumerable());

    public Task InsertAsync(Category entity)
    {
        Categories.Add(entity);
        return Task.CompletedTask;
    }

    public void SetFakeCategory(Category category)
    {
        Categories.Clear(); // assuming Categories is a List<Category>
        Categories.Add(category);
    }

    public Category Update(Category entity)
    {
        var existing = Categories.FirstOrDefault(c => c.Id == entity.Id);
        if (existing != null)
        {
            Categories.Remove(existing);
        }
        Categories.Add(entity);
        return entity;
    }

    public void SoftDelete(Category entity)
    {
        entity.MarkAsDeleted();
    }

    public IQueryable<Category> Where(Expression<Func<Category, bool>> predicate)
    {
        return Categories.AsQueryable().Where(predicate);
    }

}
