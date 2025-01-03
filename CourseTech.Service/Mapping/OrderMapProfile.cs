using AutoMapper;
using CourseTech.Core.DTOs.Order;
using CourseTech.Core.Models;

namespace CourseTech.Service.Mapping
{
    public class OrderMapProfile : Profile
    {
        public OrderMapProfile()
        {
            CreateMap<Order, OrderDTO>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.AppUser.FirstName} {src.AppUser.LastName}"))
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));

            CreateMap<OrderItem, OrderItemDTO>()
               .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Course.Title))
               .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
               .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Course.ImageUrl));
        }
    }
}