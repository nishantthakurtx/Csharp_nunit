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

                var instructorUser1 = new AppUser
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Fatih",
                    LastName = "Cakiroglu",
                    Email = "fatihcakiroglu@instructor.com",
                    EmailConfirmed = true,
                    UserName = "fatihcakiroglu@instructor.com"
                };

                var instructorUser2 = new AppUser
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Ahmet",
                    LastName = "Kaya",
                    Email = "ahmetkaya@instructor.com",
                    EmailConfirmed = true,
                    UserName = "ahmet@instructor.com"
                };

                await _userManager.CreateAsync(instructorUser1, "Fatih123*");
                await _userManager.CreateAsync(instructorUser2, "Ahmet123*");

                await _userManager.AddToRoleAsync(instructorUser1, Roles.Instructor.ToString());
                await _userManager.AddToRoleAsync(instructorUser2, Roles.Instructor.ToString());
            }
        }
        private async Task SeedCategoriesAsync()
        {
            if (!await _context.Categories.AnyAsync())
            {
                var categories = new List<Category>
                {
                    new Category { Id = Guid.NewGuid(), Name = "Programming" },
                    new Category { Id = Guid.NewGuid(), Name = "Finance" },
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
                var instructor1 = await _userManager.FindByEmailAsync("fatihcakiroglu@instructor.com");
                var instructor2 = await _userManager.FindByEmailAsync("ahmetkaya@instructor.com");
                var categories = await _context.Categories.ToListAsync();

                var programmingCategory = categories.FirstOrDefault(c => c.Name == "Programming");
                var financeCategory = categories.FirstOrDefault(c => c.Name == "Finance");
                var marketingCategory = categories.FirstOrDefault(c => c.Name == "Marketing");

                if (programmingCategory == null || financeCategory == null || marketingCategory == null)
                {
                    throw new Exception("Required categories not found in the database.");
                }

                var courses = new List<Course>
               {
                   new Course
                   {
                       Id = Guid.NewGuid(),
                       Title = "C# Fundamentals",
                       Description = "Learn the basics of C# programming language",
                       ImageUrl = "https://via.placeholder.com/150",
                       VideoUrl = "https://www.youtube.com/watch?v=0pZVjJ7v4w8", 
                       CategoryId = programmingCategory.Id,
                       Level = CourseLevel.Beginner,
                       Language = CourseLanguage.English,
                       Price = 50,
                       Duration = TimeSpan.FromHours(5),
                       InstructorId = instructor1!.Id
                   },
                   new Course
                   {
                       Id = Guid.NewGuid(),
                       Title = "Advanced Stock Trading",
                       Description = "Master the art of stock market trading",
                       ImageUrl = "https://via.placeholder.com/150",
                       VideoUrl = "https://www.youtube.com/watch?v=example2",
                       CategoryId = financeCategory.Id,
                       Level = CourseLevel.Advanced,
                       Language = CourseLanguage.English,
                       Price = 199,
                       Duration = TimeSpan.FromHours(12),
                       InstructorId = instructor1.Id
                   },
                   new Course
                   {
                       Id = Guid.NewGuid(),
                       Title = "Digital Marketing Essentials",
                       Description = "Learn the fundamentals of digital marketing",
                       ImageUrl = "https://via.placeholder.com/150",
                       VideoUrl = "https://www.youtube.com/watch?v=example3",
                       CategoryId = marketingCategory.Id,
                       Level = CourseLevel.Beginner,
                       Language = CourseLanguage.English,
                       Price = 79,
                       Duration = TimeSpan.FromHours(6),
                       InstructorId = instructor1.Id
                   },
                   new Course
                   {
                       Id = Guid.NewGuid(),
                       Title = "Python for Data Analysis",
                       Description = "Learn Python programming for financial analysis",
                       ImageUrl = "https://via.placeholder.com/150",
                       VideoUrl = "https://www.youtube.com/watch?v=example4",
                       CategoryId = programmingCategory.Id,
                       Level = CourseLevel.Intermediate,
                       Language = CourseLanguage.English,
                       Price = 149,
                       Duration = TimeSpan.FromHours(8),
                       InstructorId = instructor2!.Id
                   },
                   new Course
                   {
                       Id = Guid.NewGuid(),
                       Title = "Investment Banking Fundamentals",
                       Description = "Introduction to investment banking concepts",
                       ImageUrl = "https://via.placeholder.com/150",
                       VideoUrl = "https://www.youtube.com/watch?v=example5",
                       CategoryId = financeCategory.Id,
                       Level = CourseLevel.Beginner,
                       Language = CourseLanguage.English,
                       Price = 129,
                       Duration = TimeSpan.FromHours(10),
                       InstructorId = instructor2.Id
                   },
                   new Course
                   {
                       Id = Guid.NewGuid(),
                       Title = "Social Media Marketing",
                       Description = "Master social media marketing strategies",
                       ImageUrl = "https://via.placeholder.com/150",
                       VideoUrl = "https://www.youtube.com/watch?v=example6",
                       CategoryId = marketingCategory.Id,
                       Level = CourseLevel.Intermediate,
                       Language = CourseLanguage.English,
                       Price = 89,
                       Duration = TimeSpan.FromHours(7),
                       InstructorId = instructor2.Id
                   },
                   new Course
                   {
                       Id = Guid.NewGuid(),
                       Title = "JavaScript Advanced",
                       Description = "Advanced concepts in JavaScript development",
                       ImageUrl = "https://via.placeholder.com/150",
                       VideoUrl = "https://www.youtube.com/watch?v=example7",
                       CategoryId = programmingCategory.Id,
                       Level = CourseLevel.Advanced,
                       Language = CourseLanguage.English,
                       Price = 179,
                       Duration = TimeSpan.FromHours(15),
                       InstructorId = instructor2.Id
                   },
                   new Course
                   {
                       Id = Guid.NewGuid(),
                       Title = "Cryptocurrency Trading",
                       Description = "Learn to trade cryptocurrencies effectively",
                       ImageUrl = "https://via.placeholder.com/150",
                       VideoUrl = "https://www.youtube.com/watch?v=example8",
                       CategoryId = financeCategory.Id,
                       Level = CourseLevel.Intermediate,
                       Language = CourseLanguage.English,
                       Price = 159,
                       Duration = TimeSpan.FromHours(9),
                       InstructorId = instructor1.Id
                   },
                   new Course
                   {
                       Id = Guid.NewGuid(),
                       Title = "Email Marketing Mastery",
                       Description = "Master email marketing campaigns",
                       ImageUrl = "https://via.placeholder.com/150",
                       VideoUrl = "https://www.youtube.com/watch?v=example9",
                       CategoryId = marketingCategory.Id,
                       Level = CourseLevel.Advanced,
                       Language = CourseLanguage.English,
                       Price = 139,
                       Duration = TimeSpan.FromHours(8),
                       InstructorId = instructor2.Id
                   },
                   new Course
                   {
                       Id = Guid.NewGuid(),
                       Title = "React.js Fundamentals",
                       Description = "Learn the basics of React.js framework",
                       ImageUrl = "https://via.placeholder.com/150",
                       VideoUrl = "https://www.youtube.com/watch?v=example10",
                       CategoryId = programmingCategory.Id,
                       Level = CourseLevel.Beginner,
                       Language = CourseLanguage.English,
                       Price = 99,
                       Duration = TimeSpan.FromHours(6),
                       InstructorId = instructor2.Id
                   }
               };

                await _context.Courses.AddRangeAsync(courses);
                await _context.SaveChangesAsync();
            }
        }
    }
}