using CourseTech.Core.Models;
using CourseTech.Shared.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CourseTech.Repository.Seeds
{
    public class DataSeeder(AppDbContext _context, UserManager<AppUser> _userManager, RoleManager<AppRole> _roleManager)
    {
        public async Task SeedAsync()
        {
            await SeedRolesAsync();
            await SeedUsersAsync();
            await SeedCategoriesAsync();
            await SeedCoursesAsync();
        }
        private async Task SeedRolesAsync()
        {
            foreach (var role in Enum.GetNames(typeof(Roles)))
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new AppRole
                    {
                        Name = role,
                        NormalizedName = role.ToUpper()
                    });
                }
            }
        }
        private async Task SeedUsersAsync()
        {
            if (!await _userManager.Users.AnyAsync())
            {
                var adminUser = new AppUser
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Admin",
                    LastName = "Admin",
                    Email = "admin@admin.com",
                    EmailConfirmed = true,
                    UserName = "admin@admin.com",
                    PhoneNumber = "1234567890",
                    PhoneNumberConfirmed = true
                };

                await _userManager.CreateAsync(adminUser, "Admin123*");
                await _userManager.AddToRoleAsync(adminUser, Roles.Admin.ToString());

                var instructorUser = new AppUser
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Instructor",
                    LastName = "Instructor",
                    Email = "instructor@instructor.com",
                    EmailConfirmed = true,
                    UserName = "instructor@instructor.com"
                };

                await _userManager.CreateAsync(instructorUser, "Instructor123*");
                await _userManager.AddToRoleAsync(instructorUser, Roles.Instructor.ToString());
            }
        }
        private async Task SeedCategoriesAsync()
        {
            if (!await _context.Categories.AnyAsync())
            {
                var categories = new List<Category>
                {
                    new Category { Id = Guid.NewGuid(), Name = "Programming" },
                    new Category { Id = Guid.NewGuid(), Name = "Design" },
                    new Category { Id = Guid.NewGuid(), Name = "Marketing" }
                };

                await _context.Categories.AddRangeAsync(categories);
                await _context.SaveChangesAsync();
            }
        }
        private async Task SeedCoursesAsync()
        {
            if (!await _context.Courses.AnyAsync())
            {
                var instructor = await _userManager.FindByEmailAsync("instructor@instructor.com");
                var category = await _context.Categories.FirstOrDefaultAsync(c => c.Name == "Programming");

                var courses = new List<Course>
                {
                        new Course
                        {
                            Id = Guid.NewGuid(),
                            Title = "C# Fundamentals",
                            Description = "Learn the basics of C# programming language",
                            ImageUrl = "https://via.placeholder.com/150",
                            VideoUrl = "https://www.youtube.com/watch?v=0pZVjJ7v4w8",
                            CategoryId = category!.Id,
                            Level = CourseLevel.Beginner,
                            Language = CourseLanguage.English,
                            Price = 50,
                            Duration = TimeSpan.FromHours(5),
                            InstructorId = instructor!.Id,
                            IsPublished = true
                        },
                        new Course
                        {
                            Id = Guid.NewGuid(),
                            Title = "C# Intermediate",
                            Description = "Learn the intermediate concepts of C# programming language",
                            ImageUrl = "https://via.placeholder.com/150",
                            VideoUrl = "https://www.youtube.com/watch?v=0pZVjJ7v4w8",
                            CategoryId = category.Id,
                            Level = CourseLevel.Intermediate,
                            Language = CourseLanguage.English,
                            Price = 100,
                            Duration = TimeSpan.FromHours(7),
                            InstructorId = instructor.Id,
                            IsPublished = true
                        },
                        new Course
                        {
                            Id = Guid.NewGuid(),
                            Title = "C# Advanced",
                            Description = "Learn the advanced concepts of C# programming language",
                            ImageUrl = "https://via.placeholder.com/150",
                            VideoUrl = "https://www.youtube.com/watch?v=0pZVjJ7v4w8",
                            CategoryId = category.Id,
                            Level = CourseLevel.Advanced,
                            Language = CourseLanguage.English,
                            Price = 200,
                            Duration = TimeSpan.FromHours(10),
                            InstructorId = instructor.Id,
                            IsPublished = true
                        }
                };

                await _context.Courses.AddRangeAsync(courses);
                await _context.SaveChangesAsync();
            }
        }
    }
}