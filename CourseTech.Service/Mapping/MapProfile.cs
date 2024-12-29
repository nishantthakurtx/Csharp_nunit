using AutoMapper;
using CourseTech.Core.DTOs.AppUser;
using CourseTech.Core.Models;

namespace CourseTech.Service.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {

            // AppUser Mapping
            CreateMap<AppUserDTO, AppUser>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.NormalizedUserName, opt => opt.MapFrom(src => src.Email.ToUpper()))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));


            CreateMap<AppUser, AppUserDTO>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));

        }
    }
}