
using CourseTech.API.Controllers;
using CourseTech.Core.DTOs.Payment;
using CourseTech.Core.Services;
using CourseTech.Shared;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Net;
using CourseTech.Shared.Enums;

namespace CourseTech.Tests.Controllers
{
    [TestFixture]
    public class PaymentsControllerTests
    {
        private Mock<IPaymentService> _paymentServiceMock;
        private PaymentsController _controller;

        [SetUp]
        public void Setup()
        {
            _paymentServiceMock = new Mock<IPaymentService>();
            _controller = new PaymentsController(_paymentServiceMock.Object);
        }
        #region ProcessPaymentAsync
        [Test]
        public async Task ProcessPaymentAsync_ReturnsOk_WhenPaymentSuccessful()
        {
            // Arrange
            var paymentRequest = new PaymentRequestDTO(
                OrderId: Guid.NewGuid(),
                CardHolderName: "John Doe",
                CardNumber: "4111111111111111",
                ExpiryMonth: "12",
                ExpiryYear: "2030",
                Cvv: "123",
                TotalAmount: 500,
                PaymentDate: DateTime.UtcNow
            );

            var paymentDto = new PaymentDTO(
                Id: Guid.NewGuid(),
                UserId: Guid.NewGuid(),
                OrderId: paymentRequest.OrderId,
                TransactionId: "txn123",
                TotalAmount: paymentRequest.TotalAmount,
                PaymentProvider: "FakeProvider",
                IsSuccessful: true,
                PaymentDate: paymentRequest.PaymentDate,
                Status: PaymentStatus.Success
            );

            var serviceResult = ServiceResult<PaymentDTO>.Success(paymentDto);

            _paymentServiceMock
                .Setup(s => s.ProcessPaymentAsync(paymentRequest))
                .ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.ProcessPaymentAsync(paymentRequest);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.AreEqual((int)HttpStatusCode.OK, objectResult!.StatusCode);
            Assert.AreEqual(serviceResult, objectResult.Value);
        }

        [Test]
        public async Task ProcessPaymentAsync_ReturnsBadRequest_WhenOrderNotFound()
        {
            // Arrange
            var paymentRequest = new PaymentRequestDTO(
                OrderId: Guid.NewGuid(),
                CardHolderName: "John Doe",
                CardNumber: "4111111111111111",
                ExpiryMonth: "12",
                ExpiryYear: "2030",
                Cvv: "123",
                TotalAmount: 500,
                PaymentDate: DateTime.UtcNow
            );

            var serviceResult = ServiceResult<PaymentDTO>.Fail("Order not found.", HttpStatusCode.BadRequest);

            _paymentServiceMock
                .Setup(s => s.ProcessPaymentAsync(paymentRequest))
                .ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.ProcessPaymentAsync(paymentRequest);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.AreEqual((int)HttpStatusCode.BadRequest, objectResult!.StatusCode);
            Assert.AreEqual(serviceResult, objectResult.Value);
        }

        [Test]
        public async Task ProcessPaymentAsync_ReturnsServerError_WhenUnhandledExceptionOccurs()
        {
            // Arrange
            var paymentRequest = new PaymentRequestDTO(
                OrderId: Guid.NewGuid(),
                CardHolderName: "John Doe",
                CardNumber: "4111111111111111",
                ExpiryMonth: "12",
                ExpiryYear: "2030",
                Cvv: "123",
                TotalAmount: 500,
                PaymentDate: DateTime.UtcNow
            );

            _paymentServiceMock
                .Setup(s => s.ProcessPaymentAsync(paymentRequest))
                .ThrowsAsync(new Exception("Unexpected failure"));

            // Act & Assert
            Assert.ThrowsAsync<Exception>(() => _controller.ProcessPaymentAsync(paymentRequest));
        }
#endregion

        #region GetPaymentsByUserId
        [Test]
        public async Task GetPaymentsByUserId_ReturnsOk_WhenPaymentsExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var payments = new List<PaymentDTO>
            {
                new PaymentDTO(Guid.NewGuid(), userId, Guid.NewGuid(), "txn001", 199, "FakePay", true, DateTime.UtcNow, PaymentStatus.Success)
            };

            var serviceResult = ServiceResult<List<PaymentDTO>>.Success(payments);
            _paymentServiceMock.Setup(s => s.GetPaymentsByUserAsync(userId)).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.GetPaymentsByUserId(userId);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.OK, objectResult.StatusCode);
            Assert.AreEqual(serviceResult, objectResult.Value);
        }

        [Test]
        public async Task GetPaymentsByUserId_ReturnsBadRequest_WhenPaymentsNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var serviceResult = ServiceResult<List<PaymentDTO>>.Fail("Payments not found.");

            _paymentServiceMock.Setup(s => s.GetPaymentsByUserAsync(userId)).ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.GetPaymentsByUserId(userId);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, objectResult.StatusCode);
            Assert.AreEqual(serviceResult, objectResult.Value);
        }
        #endregion

        #region GetPaymentById
        [Test]
        public async Task GetPaymentById_ReturnsOk_WhenFound()
        {
            // Arrange
            var paymentId = Guid.NewGuid();
            var dto = new PaymentDTO(
                paymentId,
                Guid.NewGuid(), // userId
                Guid.NewGuid(), // orderId
                "txn-123",
                999.00m,
                "TestPay",
                true,
                DateTime.UtcNow,
                PaymentStatus.Success
            );

            var serviceResult = ServiceResult<PaymentDTO>.Success(dto);

            _paymentServiceMock
                .Setup(s => s.GetPaymentByIdAsync(paymentId))
                .ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.GetPaymentById(paymentId);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.OK, objectResult.StatusCode);
            Assert.AreEqual(serviceResult, objectResult.Value);
        }

        [Test]
        public async Task GetPaymentById_ReturnsBadRequest_WhenNotFound()
        {
            // Arrange
            var paymentId = Guid.NewGuid();
            var failResult = ServiceResult<PaymentDTO>.Fail("Payment not found.");

            _paymentServiceMock
                .Setup(s => s.GetPaymentByIdAsync(paymentId))
                .ReturnsAsync(failResult);

            // Act
            var result = await _controller.GetPaymentById(paymentId);

            // Assert
            var objectResult = result as ObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, objectResult.StatusCode);
            Assert.AreEqual(failResult, objectResult.Value);
        }
        // edge case unexpected error
        #endregion
    }
}
