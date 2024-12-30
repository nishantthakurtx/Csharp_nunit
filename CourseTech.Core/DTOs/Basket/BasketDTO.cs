using CourseTech.Shared.Enums;

namespace CourseTech.Core.DTOs.Basket
{
    public record BasketDTO(
        Guid Id,
        Guid UserId,
        List<BasketItemDTO> Items,
        BasketStatus Status,
        decimal TotalPrice
    );
}