using System.Net;
using CourseTech.API.Controllers;
using CourseTech.Core.DTOs.Payment;
using CourseTech.Core.Services;
using CourseTech.Shared;
using CourseTech.Tests.Helpers;
using CourseTech.Tests.TestData;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CourseTech.Tests.Controllers
{
    public class PaymentsControllerTests
    {
        private readonly Mock<IPaymentService> _paymentServiceMock;
        private readonly PaymentsController _controller;

        public PaymentsControllerTests()
        {
            _paymentServiceMock = new(MockBehavior.Strict);
            _controller = new PaymentsController(_paymentServiceMock.Object);
        }

        [Fact]
        public async Task ProcessPaymentAsync_ReturnsOk_WhenSuccessful()
        {
            var request = PaymentTestData.GetSampleRequest();
            var response = PaymentTestData.GetSampleDTO();

            PaymentServiceMockHelper.SetupProcessPaymentSuccess(_paymentServiceMock, request, response);

            var result = await _controller.ProcessPaymentAsync(request);
            var objectResult = Assert.IsType<ObjectResult>(result);

            Assert.Equal((int)HttpStatusCode.OK, objectResult.StatusCode);

            var value = Assert.IsType<ServiceResult<PaymentDTO>>(objectResult.Value);
            Assert.Equal(response.TransactionId, value.Data!.TransactionId);
        }

        [Fact]
        public async Task ProcessPaymentAsync_ReturnsBadRequest_WhenOrderNotFound()
        {
            var request = PaymentTestData.GetSampleRequest();
            PaymentServiceMockHelper.SetupProcessPaymentFail(_paymentServiceMock, request, TestConstants.ErrorMessageOrderNotFound);

            var result = await _controller.ProcessPaymentAsync(request);
            var objectResult = Assert.IsType<ObjectResult>(result);

            Assert.Equal((int)HttpStatusCode.BadRequest, objectResult.StatusCode);
            var value = Assert.IsType<ServiceResult<PaymentDTO>>(objectResult.Value);
            Assert.Equal(TestConstants.ErrorMessageOrderNotFound, value.ErrorMessage!.FirstOrDefault());
        }

        [Fact]
        public async Task ProcessPaymentAsync_Throws_WhenExceptionOccurs()
        {
            var request = PaymentTestData.GetSampleRequest();
            _paymentServiceMock.Setup(x => x.ProcessPaymentAsync(request)).ThrowsAsync(new Exception("Unexpected"));

            await Assert.ThrowsAsync<Exception>(() => _controller.ProcessPaymentAsync(request));
        }

        [Fact]
        public async Task GetPaymentsByUserId_ReturnsOk_WhenFound()
        {
            var list = new List<PaymentDTO> { PaymentTestData.GetSampleDTO() };

            
            PaymentServiceMockHelper.SetupGetPaymentsByUser(_paymentServiceMock, TestConstants.SampleUserId, list);

            var result = await _controller.GetPaymentsByUserId(TestConstants.SampleUserId);
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, objectResult.StatusCode);
        }


        [Fact]
        public async Task GetPaymentsByUserId_ReturnsBadRequest_WhenNotFound()
        {
            var fail = ServiceResult<List<PaymentDTO>>.Fail(TestConstants.ErrorMessagePaymentNotFound);
            _paymentServiceMock.Setup(s => s.GetPaymentsByUserAsync(TestConstants.SampleUserId)).ReturnsAsync(fail);

            var result = await _controller.GetPaymentsByUserId(TestConstants.SampleUserId);
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, objectResult.StatusCode);
        }

        [Fact]
        public async Task GetPaymentById_ReturnsOk_WhenFound()
        {
            var dto = PaymentTestData.GetSampleDTO();
            _paymentServiceMock.Setup(s => s.GetPaymentByIdAsync(TestConstants.SamplePaymentId))
                .ReturnsAsync(ServiceResult<PaymentDTO>.Success(dto));

            var result = await _controller.GetPaymentById(TestConstants.SamplePaymentId);
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, objectResult.StatusCode);
        }

        [Fact]
        public async Task GetPaymentById_ReturnsBadRequest_WhenNotFound()
        {
            PaymentServiceMockHelper.SetupGetPaymentByIdFail(_paymentServiceMock, TestConstants.SamplePaymentId, TestConstants.ErrorMessagePaymentNotFound);

            var result = await _controller.GetPaymentById(TestConstants.SamplePaymentId);
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, objectResult.StatusCode);
        }
    }
}
