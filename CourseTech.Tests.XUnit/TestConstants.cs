using System;

namespace CourseTech.Tests.Helpers
{
    public static class TestConstants
    {
        public static readonly Guid SampleUserId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        public static readonly Guid SampleCourseId = Guid.Parse("22222222-2222-2222-2222-222222222222");
        public static readonly Guid SampleBasketId = Guid.Parse("33333333-3333-3333-3333-333333333333");

        public const string ErrorMessageCourseNotFound = "Course not found";
        public const string ErrorMessageCourseNotInBasket = "Course not found in basket";
        public const string ErrorMessageBasketNotFound = "Basket not found.";
        public const string ErrorMessageCompleteBasketFailed = "No active basket to complete.";


        //payment
        public static readonly Guid SampleOrderId = Guid.Parse("22222222-2222-2222-2222-222222222222");
        public static readonly Guid SamplePaymentId = Guid.Parse("33333333-3333-3333-3333-333333333333");

        public const string ErrorMessageOrderNotFound = "Order not found.";
        public const string ErrorMessagePaymentNotFound = "Payment not found.";

        //category
        public static readonly Guid SampleInstructorId = Guid.Parse("44444444-4444-4444-4444-444444444444");
        public static readonly Guid SampleCategoryId = Guid.Parse("55555555-5555-5555-5555-555555555555");


        public const string ErrorMessageInstructorNotFound = "Instructor not found";
        public const string ErrorMessageCategoryNotFound = "Category not found";
    }
}
