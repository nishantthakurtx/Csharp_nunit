using AutoMapper;
using CourseTech.Core.Models;
using CourseTech.Core.Repositories;
using CourseTech.Core.Services;
using CourseTech.Core.UnitOfWorks;
using CourseTech.Repository;
using CourseTech.Repository.Repositories;
using CourseTech.Repository.Seeds;
using CourseTech.Service.Services;
using CourseTech.Shared.Configuration;
using CourseTech.Shared.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Reflection;
using System.Text.Json.Serialization;

namespace CourseTech.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddOpenApi();

            // Add services to the container.
            builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            // hatali
            builder.Services.AddValidatorsFromAssembly(typeof(AbstractValidator<>).Assembly);

            #region DbContext Configuration
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
            });
            #endregion
            #region Service Extensions
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<ICourseService, CourseService>();
            builder.Services.AddScoped<IBasketService, BasketService>();
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            #endregion
            #region Identity Configuration
            builder.Services.AddIdentity<AppUser, AppRole>(opt =>
            {
                opt.User.RequireUniqueEmail = true;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredLength = 6;

            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
 
            builder.Services.Configure<TokenOption>(builder.Configuration.GetSection("TokenOption"));

            builder.Services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
            {
                var tokenOption = builder.Configuration.GetSection("TokenOption").Get<TokenOption>()!;
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = tokenOption.Issuer,
                    ValidAudience = tokenOption.Audience[0],
                    IssuerSigningKey = SigningService.GetSymmetricSecurityKey(tokenOption.SecurityKey),

                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                };
            });
            #endregion

            var app = builder.Build();

            #region Seed Data
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

                var seeder = new DataSeeder(dbContext, userManager, roleManager);
                await seeder.SeedAsync();
            }
            #endregion
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference(opt =>
                {
                    opt.WithSidebar(false);
                    opt.WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            await app.RunAsync();
        }
    }
}