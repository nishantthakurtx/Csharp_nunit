using AutoMapper;
using CourseTech.Core.DTOs.AppUser;
using CourseTech.Core.DTOs.Course;
using CourseTech.Core.Models;

namespace CourseTech.Tests.Helpers.Mapping
{
    public class TestMappingProfile : Profile
    {
        public TestMappingProfile()
        {
            // === Course mappings ===

            CreateMap<CourseDTO, Course>()
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.Instructor, opt => opt.Ignore());

            CreateMap<Course, CourseDTO>()
              .ConstructUsing(src => new CourseDTO(
                  src.Id,
                  src.Title,
                  src.Description,
                  src.ImageUrl,
                  src.VideoUrl,
                  src.Level.ToString(),
                  src.Language.ToString(),
                  src.Price,
                  src.Duration,
                  src.PublishedAt,
                  (src.Instructor != null ? src.Instructor.FirstName : "") + " " +
                  (src.Instructor != null ? src.Instructor.LastName : ""),
                  src.Category != null ? src.Category.Name : "",
                  src.CreatedAt
              ));

            CreateMap<Course, CourseSummaryDTO>()
                .ConstructUsing(src => new CourseSummaryDTO(
                    src.Id,
                    src.Title,
                    src.Price,
                    src.ImageUrl,
                    (src.Instructor != null ? src.Instructor.FirstName : "") + " " +
                    (src.Instructor != null ? src.Instructor.LastName : ""),
                    src.Level.ToString()
                ));

            CreateMap<Course, CourseListDTO>()
                .ConstructUsing(src => new CourseListDTO(
                    src.Id,
                    src.Title,
                    src.Description,
                    src.Price,
                    src.ImageUrl,
                    src.Category != null ? src.Category.Name : "",
                    (src.Instructor != null ? src.Instructor.FirstName : "") + " " +
                    (src.Instructor != null ? src.Instructor.LastName : ""),
                    src.Duration,
                    src.Level.ToString(),
                    src.IsPublished
                ));

            CreateMap<CourseCreateDTO, Course>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.InstructorId, opt => opt.MapFrom(src => src.InstructorId))
                .ForMember(dest => dest.IsPublished, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.PublishedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Instructor, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore());

            CreateMap<CourseUpdateDTO, Course>()
                .ForMember(dest => dest.IsPublished, opt => opt.Ignore())
                .ForMember(dest => dest.PublishedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.Instructor, opt => opt.Ignore());

            // === AppUser mappings ===

            CreateMap<AppUser, AppUserDTO>()
              .ConstructUsing(src => new AppUserDTO(
                  src.Id,
                  src.FirstName,
                  src.LastName,
                  src.Email ?? "",
                  src.PhoneNumber ?? ""
              ))
              .ReverseMap(); 

            CreateMap<AppUserCreateDTO, AppUser>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));

           
            CreateMap<AppUser, AppUserWithNamesDTO>()
                .ConstructUsing(src => new AppUserWithNamesDTO(
                    src.Id,
                    src.FirstName + " " + src.LastName
                ));
        }
    }
}
