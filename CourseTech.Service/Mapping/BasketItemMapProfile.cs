using AutoMapper;
using CourseTech.Core.DTOs.Basket;
using CourseTech.Core.Models;

namespace CourseTech.Service.Mapping
{
    public class BasketItemMapProfile : Profile
    {
        public BasketItemMapProfile() 
        {
            CreateMap<BasketItem, BasketItemDTO>()
                .ForCtorParam("BasketId", opt => opt.MapFrom(src => src.BasketId))
                .ForCtorParam("CourseId", opt => opt.MapFrom(src => src.CourseId))
                .ForCtorParam("CourseTitle", opt => opt.MapFrom(src => src.Course.Title))
                .ForCtorParam("Price", opt => opt.MapFrom(src => src.Course.Price));
        }
    }
}