using AutoMapper;
using CourseTech.Core.DTOs.Order;
using CourseTech.Core.Models;
using CourseTech.Core.Services;
using CourseTech.Core.UnitOfWorks;
using CourseTech.Shared;
using CourseTech.Shared.Enums;
using Microsoft.Extensions.Logging;

namespace CourseTech.Service.Services
{
    public class OrderService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<OrderService> logger) : IOrderService
    {
        public async Task<ServiceResult<OrderDTO>> CreateOrderFromBasketAsync(Guid basketId)
        {
            var basket = await unitOfWork.Basket.GetByIdAsync(basketId);
            if (basket == null)
                return ServiceResult<OrderDTO>.Fail("Basket not found.");

            if (basket.Status != BasketStatus.Passive)
                return ServiceResult<OrderDTO>.Fail("Basket is not passive.");

            if (!basket.BasketItems.Any())
                return ServiceResult<OrderDTO>.Fail("Basket is empty.");

            var order = new Order(basket.UserId, basket.AppUser);

            foreach (var basketItem in basket.BasketItems)
            {
                order.AddOrderItem(basketItem.Course);
            }

            await unitOfWork.Order.InsertAsync(order);
            unitOfWork.Basket.SoftDelete(basket);
            await unitOfWork.SaveChangesAsync();

            var orderDTO = mapper.Map<OrderDTO>(order);

            logger.LogInformation("Order {OrderId} created from basket {BasketId}", order.Id, basket.Id);
            return ServiceResult<OrderDTO>.Success(orderDTO);
        }

        public async Task<ServiceResult<OrderDTO>> GetOrderByIdAsync(Guid orderId)
        {
            var order = await unitOfWork.Order.GetOrderByIdWithIncludesAsync(orderId);
            if (order == null)
                return ServiceResult<OrderDTO>.Fail("Order not found.");

            var orderDTO = mapper.Map<OrderDTO>(order);
            return ServiceResult<OrderDTO>.Success(orderDTO);
        }

        public async Task<ServiceResult<List<OrderSummaryDTO>>> GetOrdersByUserIdAsync(Guid userId)
        {
            var orders = await unitOfWork.Order.GetOrdersByUserIdAsync(userId);
            var orderDTOs = mapper.Map<List<OrderSummaryDTO>>(orders);
            return ServiceResult<List<OrderSummaryDTO>>.Success(orderDTOs);
        }
    }
}