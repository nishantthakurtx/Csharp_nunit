using CourseTech.Core.DTOs.Payment;
using CourseTech.Core.Services;
using CourseTech.Shared;
using Moq;
using System.Net;

namespace CourseTech.Tests.Helpers
{
    public static class PaymentServiceMockHelper
    {
        public static void SetupProcessPaymentSuccess(Mock<IPaymentService> mock, PaymentRequestDTO request, PaymentDTO response)
        {
            mock.Setup(x => x.ProcessPaymentAsync(request)).ReturnsAsync(ServiceResult<PaymentDTO>.Success(response));
        }

        public static void SetupProcessPaymentFail(Mock<IPaymentService> mock, PaymentRequestDTO request, string error)
        {
            mock.Setup(x => x.ProcessPaymentAsync(request)).ReturnsAsync(ServiceResult<PaymentDTO>.Fail(error, HttpStatusCode.BadRequest));
        }

        public static void SetupGetPaymentsByUser(Mock<IPaymentService> mock, Guid userId, List<PaymentDTO> list)
        {
            mock.Setup(x => x.GetPaymentsByUserAsync(userId)).ReturnsAsync(ServiceResult<List<PaymentDTO>>.Success(list));
        }

        public static void SetupGetPaymentByIdFail(Mock<IPaymentService> mock, Guid paymentId, string error)
        {
            mock.Setup(x => x.GetPaymentByIdAsync(paymentId)).ReturnsAsync(ServiceResult<PaymentDTO>.Fail(error, HttpStatusCode.BadRequest));
        }
    }
}
