using CourseTech.Core.DTOs.Payment;
using CourseTech.Shared.Enums;
using CourseTech.Tests.Helpers;

namespace CourseTech.Tests.TestData
{
    public static class PaymentTestData
    {
        public static PaymentRequestDTO GetSampleRequest() =>
            new(
                OrderId: TestConstants.SampleOrderId,
                CardHolderName: "John Doe",
                CardNumber: "4111111111111111",
                ExpiryMonth: "12",
                ExpiryYear: "2030",
                Cvv: "123",
                TotalAmount: 500,
                PaymentDate: DateTime.UtcNow
            );

        public static PaymentDTO GetSampleDTO() =>
            new(
                Id: TestConstants.SamplePaymentId,
                UserId: TestConstants.SampleUserId,
                OrderId: TestConstants.SampleOrderId,
                TransactionId: "txn-123",
                TotalAmount: 500,
                PaymentProvider: "TestProvider",
                IsSuccessful: true,
                PaymentDate: DateTime.UtcNow,
                Status: PaymentStatus.Success
            );
    }
}
