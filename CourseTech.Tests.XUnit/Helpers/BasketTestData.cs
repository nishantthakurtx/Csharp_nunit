using CourseTech.Core.DTOs.Basket;
using System.Collections.Generic;

namespace CourseTech.Tests.Helpers
{
    public static class BasketTestData
    {
        public static BasketDTO GetSampleBasketDTO()
        {
            return new BasketDTO(
                TestConstants.SampleBasketId,
                TestConstants.SampleUserId,
                new List<BasketItemDTO>(),
                "Active",
                199.99m
            );
        }
    }
}
