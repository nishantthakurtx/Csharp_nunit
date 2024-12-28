using AutoMapper;
using CourseTech.Core.DTOs.AppUser;
using CourseTech.Core.DTOs.Course;
using CourseTech.Core.Models;

namespace CourseTech.Service.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            // Course Mapping
            CreateMap<Course,CourseDto>().ReverseMap();

            // AppUser Mapping
            CreateMap<AppUserDto, AppUser>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.NormalizedUserName, opt => opt.MapFrom(src => src.Email.ToUpper()))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));


            CreateMap<AppUser, AppUserDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));

        }
    }
}