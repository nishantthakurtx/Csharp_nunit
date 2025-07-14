using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CourseTech.API.Controllers;
using CourseTech.Core.DTOs.Basket;
using CourseTech.Core.Services;
using CourseTech.Shared;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace CourseTech.Tests.Controllers
{
    [TestFixture]
    public class BasketsControllerTests
    {
        private Mock<IBasketService> _basketServiceMock;
        private BasketsController _controller;

        [SetUp]
        public void Setup()
        {
            _basketServiceMock = new Mock<IBasketService>();
            _controller = new BasketsController(_basketServiceMock.Object);
        }

        #region Get Basket
        [Test]
        public async Task GetBasket_ReturnsOk_WithBasket()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var basketDto = new BasketDTO(
                Guid.NewGuid(),              // Id
                userId,                      // UserId
                new List<BasketItemDTO>(),   // Items
                "Active",                    // Status
                199.99m                      // TotalPrice
            );

            var serviceResult = ServiceResult<BasketDTO>.Success(basketDto);

            _basketServiceMock
                .Setup(x => x.GetActiveBasketAsync(userId))
                .ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.GetBasket(userId);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);

            var objectResult = result as ObjectResult;
            Assert.AreEqual(200, objectResult!.StatusCode);
            Assert.AreEqual(serviceResult, objectResult.Value);
        }

        #endregion

        #region AddCourse

        [Test]
        public async Task AddCourseToBasket_ReturnsOk_WhenSuccessful()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var courseId = Guid.NewGuid();

            var serviceResult = ServiceResult.Success();

            _basketServiceMock
                .Setup(s => s.AddCourseToBasketAsync(userId, courseId))
                .ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.AddCourseToBasket(userId, courseId);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);

            var objectResult = result as ObjectResult;
            Assert.AreEqual(200, objectResult!.StatusCode);
            Assert.AreEqual(serviceResult, objectResult.Value);
        }

        //course not found
        [Test]
        public async Task AddCourseToBasket_ReturnsBadRequest_WhenFailed()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var courseId = Guid.NewGuid();

            var serviceResult = ServiceResult.Fail("Course not found");

            _basketServiceMock
                .Setup(s => s.AddCourseToBasketAsync(userId, courseId))
                .ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.AddCourseToBasket(userId, courseId);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);

            var objectResult = result as ObjectResult;
            Assert.AreEqual(400, objectResult!.StatusCode);
            Assert.AreEqual(serviceResult, objectResult.Value);
        }

        #endregion

        #region Remove basket

        [Test]
        public async Task RemoveCourseFromBasket_ReturnsOk_WhenSuccessful()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var courseId = Guid.NewGuid();

            var serviceResult = ServiceResult.Success();

            _basketServiceMock
                .Setup(s => s.RemoveCourseFromBasketAsync(userId, courseId))
                .ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.RemoveCourseFromBasket(userId, courseId);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);

            var objectResult = result as ObjectResult;
            Assert.AreEqual(200, objectResult!.StatusCode);
            Assert.AreEqual(serviceResult, objectResult.Value);
        }

        //when the course is not in basket

        //[Test]
        //public async Task RemoveCourseFromBasket_ReturnsBadRequest_WhenFailed()
        //{
        //    // Arrange
        //    var userId = Guid.NewGuid();
        //    var courseId = Guid.NewGuid();

        //    var serviceResult = ServiceResult.Fail("Course not found in basket");

        //    _basketServiceMock
        //        .Setup(s => s.RemoveCourseFromBasketAsync(userId, courseId))
        //        .ReturnsAsync(serviceResult);

        //    // Act
        //    var result = await _controller.RemoveCourseFromBasket(userId, courseId);

        //    // Assert
        //    Assert.IsInstanceOf<ObjectResult>(result);

        //    var objectResult = result as ObjectResult;
        //    Assert.AreEqual(400, objectResult!.StatusCode);
        //    Assert.AreEqual(serviceResult, objectResult.Value);
        //}
        #endregion

        #region Clear Basket
        [Test]
        public async Task ClearBasket_ReturnsOk_WhenSuccessful()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var serviceResult = ServiceResult.Success();

            _basketServiceMock
                .Setup(s => s.ClearBasketAsync(userId))
                .ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.ClearBasket(userId);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);

            var objectResult = result as ObjectResult;
            Assert.AreEqual(200, objectResult!.StatusCode);
            Assert.AreEqual(serviceResult, objectResult.Value);
        }
        //[Test]
        //public async Task ClearBasket_ReturnsBadRequest_WhenFailed()
        //{
        //    // Arrange
        //    var userId = Guid.NewGuid();
        //    var serviceResult = ServiceResult.Fail("Basket not found.");

        //    _basketServiceMock
        //        .Setup(s => s.ClearBasketAsync(userId))
        //        .ReturnsAsync(serviceResult);

        //    // Act
        //    var result = await _controller.ClearBasket(userId);

        //    // Assert
        //    Assert.IsInstanceOf<ObjectResult>(result);

        //    var objectResult = result as ObjectResult;
        //    Assert.AreEqual(400, objectResult!.StatusCode);
        //    Assert.AreEqual(serviceResult, objectResult.Value);
        //}

        #endregion

        #region Complete Basket 
        [Test]
        public async Task CompleteBasket_ReturnsOk_WhenSuccessful()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var serviceResult = ServiceResult.Success();

            _basketServiceMock
                .Setup(s => s.CompleteBasketAsync(userId))
                .ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.CompleteBasket(userId);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);

            var objectResult = result as ObjectResult;
            Assert.AreEqual(200, objectResult!.StatusCode);
            Assert.AreEqual(serviceResult, objectResult.Value);
        }
        //WhenFailed
        //[Test]
        //public async Task CompleteBasket_ReturnsBadRequest_WhenFailed()
        //{
        //    // Arrange
        //    var userId = Guid.NewGuid();
        //    var serviceResult = ServiceResult.Fail("No active basket to complete.");

        //    _basketServiceMock
        //        .Setup(s => s.CompleteBasketAsync(userId))
        //        .ReturnsAsync(serviceResult);

        //    // Act
        //    var result = await _controller.CompleteBasket(userId);

        //    // Assert
        //    Assert.IsInstanceOf<ObjectResult>(result);

        //    var objectResult = result as ObjectResult;
        //    Assert.AreEqual(400, objectResult!.StatusCode);
        //    Assert.AreEqual(serviceResult, objectResult.Value);
        //}
        #endregion

        #region admin BasketWithItems
        [Test]
        public async Task GetBasketWithItems_ReturnsOk_WhenFound()
        {
            // Arrange
            var basketId = Guid.NewGuid();
            var basketDto = new BasketDTO(
                basketId,
                Guid.NewGuid(),                // userId (not used directly here)
                new List<BasketItemDTO>(),     // Items
                "Completed",                   // Status
                299.99m                        // TotalPrice
            );
            var serviceResult = ServiceResult<BasketDTO>.Success(basketDto);

            _basketServiceMock
                .Setup(s => s.GetBasketWithItemsAsync(basketId))
                .ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.GetBasketWithItems(basketId);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            var ok = result as ObjectResult;
            Assert.AreEqual(200, ok!.StatusCode);
            Assert.AreEqual(serviceResult, ok.Value);
        }

        [Test]
        public async Task GetBasketWithItems_ReturnsBadRequest_WhenNotFound()
        {
            // Arrange
            var basketId = Guid.NewGuid();
            var serviceResult = ServiceResult<BasketDTO>.Fail("Basket not found");

            _basketServiceMock
                .Setup(s => s.GetBasketWithItemsAsync(basketId))
                .ReturnsAsync(serviceResult);

            // Act
            var result = await _controller.GetBasketWithItems(basketId);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            var bad = result as ObjectResult;
            Assert.AreEqual(400, bad!.StatusCode);
            Assert.AreEqual(serviceResult, bad.Value);
        }

        #endregion
    }
}
