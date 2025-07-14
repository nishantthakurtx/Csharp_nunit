using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using CourseTech.API.Controllers;
using CourseTech.Core.DTOs.Basket;
using CourseTech.Core.Services;
using CourseTech.Shared;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using CourseTech.Tests.Helpers;

namespace CourseTech.Tests.Controllers
{
    public class BasketsControllerTests
    {
        private readonly Mock<IBasketService> _basketServiceMock;
        private readonly BasketsController _controller;

        public BasketsControllerTests()
        {
            _basketServiceMock = new(MockBehavior.Strict);
            _controller = new BasketsController(_basketServiceMock.Object);
        }

        #region Get Basket

        [Fact]
        public async Task GetBasketAsync_WithValidUser_ReturnsOk()
        {
            var basketDto = BasketTestData.GetSampleBasketDTO();
            var expected = ServiceResult<BasketDTO>.Success(basketDto);

            _basketServiceMock.Setup(s => s.GetActiveBasketAsync(TestConstants.SampleUserId))
                              .ReturnsAsync(expected);

            var result = await _controller.GetBasket(TestConstants.SampleUserId);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, objectResult.StatusCode);

            var actual = Assert.IsType<ServiceResult<BasketDTO>>(objectResult.Value);
            Assert.True(actual.IsSuccess);
            Assert.Equal(expected.Data?.Id, actual.Data?.Id);
            Assert.Equal(expected.Status, actual.Status);
        }

        #endregion

        #region Add Course

        [Fact]
        public async Task AddCourseToBasketAsync_WhenSuccessful_ReturnsOk()
        {
            var expected = ServiceResult.Success();

            _basketServiceMock.Setup(s => s.AddCourseToBasketAsync(TestConstants.SampleUserId, TestConstants.SampleCourseId))
                              .ReturnsAsync(expected);

            var result = await _controller.AddCourseToBasket(TestConstants.SampleUserId, TestConstants.SampleCourseId);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, objectResult.StatusCode);

            var actual = Assert.IsType<ServiceResult>(objectResult.Value);
            Assert.True(actual.IsSuccess);
        }

        [Fact]
        public async Task AddCourseToBasketAsync_WhenFailed_ReturnsBadRequest()
        {
            var expected = ServiceResult.Fail(TestConstants.ErrorMessageCourseNotFound);

            _basketServiceMock.Setup(s => s.AddCourseToBasketAsync(TestConstants.SampleUserId, TestConstants.SampleCourseId))
                              .ReturnsAsync(expected);

            var result = await _controller.AddCourseToBasket(TestConstants.SampleUserId, TestConstants.SampleCourseId);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, objectResult.StatusCode);

            var actual = Assert.IsType<ServiceResult>(objectResult.Value);
            Assert.False(actual.IsSuccess);
            Assert.Equal(TestConstants.ErrorMessageCourseNotFound, actual.ErrorMessage?[0]);
        }

        #endregion

        #region Remove Course

        [Fact]
        public async Task RemoveCourseFromBasketAsync_WhenSuccessful_ReturnsOk()
        {
            var expected = ServiceResult.Success();

            _basketServiceMock.Setup(s => s.RemoveCourseFromBasketAsync(TestConstants.SampleUserId, TestConstants.SampleCourseId))
                              .ReturnsAsync(expected);

            var result = await _controller.RemoveCourseFromBasket(TestConstants.SampleUserId, TestConstants.SampleCourseId);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, objectResult.StatusCode);

            var actual = Assert.IsType<ServiceResult>(objectResult.Value);
            Assert.True(actual.IsSuccess);
        }

        [Fact]
        public async Task RemoveCourseFromBasketAsync_WhenFailed_ReturnsBadRequest()
        {
            var expected = ServiceResult.Fail(TestConstants.ErrorMessageCourseNotInBasket);

            _basketServiceMock.Setup(s => s.RemoveCourseFromBasketAsync(TestConstants.SampleUserId, TestConstants.SampleCourseId))
                              .ReturnsAsync(expected);

            var result = await _controller.RemoveCourseFromBasket(TestConstants.SampleUserId, TestConstants.SampleCourseId);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, objectResult.StatusCode);

            var actual = Assert.IsType<ServiceResult>(objectResult.Value);
            Assert.False(actual.IsSuccess);
            Assert.Equal(TestConstants.ErrorMessageCourseNotInBasket, actual.ErrorMessage?[0]);
        }

        #endregion

        #region Clear Basket

        [Fact]
        public async Task ClearBasketAsync_WhenSuccessful_ReturnsOk()
        {
            var expected = ServiceResult.Success();

            _basketServiceMock.Setup(s => s.ClearBasketAsync(TestConstants.SampleUserId))
                              .ReturnsAsync(expected);

            var result = await _controller.ClearBasket(TestConstants.SampleUserId);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, objectResult.StatusCode);

            var actual = Assert.IsType<ServiceResult>(objectResult.Value);
            Assert.True(actual.IsSuccess);
        }

        [Fact]
        public async Task ClearBasketAsync_WhenFailed_ReturnsBadRequest()
        {
            var expected = ServiceResult.Fail(TestConstants.ErrorMessageBasketNotFound);

            _basketServiceMock.Setup(s => s.ClearBasketAsync(TestConstants.SampleUserId))
                              .ReturnsAsync(expected);

            var result = await _controller.ClearBasket(TestConstants.SampleUserId);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, objectResult.StatusCode);

            var actual = Assert.IsType<ServiceResult>(objectResult.Value);
            Assert.False(actual.IsSuccess);
            Assert.Equal(TestConstants.ErrorMessageBasketNotFound, actual.ErrorMessage?[0]);
        }

        #endregion

        #region Complete Basket

        [Fact]
        public async Task CompleteBasketAsync_WhenSuccessful_ReturnsOk()
        {
            var expected = ServiceResult.Success();

            _basketServiceMock.Setup(s => s.CompleteBasketAsync(TestConstants.SampleUserId))
                              .ReturnsAsync(expected);

            var result = await _controller.CompleteBasket(TestConstants.SampleUserId);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, objectResult.StatusCode);

            var actual = Assert.IsType<ServiceResult>(objectResult.Value);
            Assert.True(actual.IsSuccess);
        }

        [Fact]
        public async Task CompleteBasketAsync_WhenFailed_ReturnsBadRequest()
        {
            var expected = ServiceResult.Fail(TestConstants.ErrorMessageCompleteBasketFailed);

            _basketServiceMock.Setup(s => s.CompleteBasketAsync(TestConstants.SampleUserId))
                              .ReturnsAsync(expected);

            var result = await _controller.CompleteBasket(TestConstants.SampleUserId);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, objectResult.StatusCode);

            var actual = Assert.IsType<ServiceResult>(objectResult.Value);
            Assert.False(actual.IsSuccess);
            Assert.Equal(TestConstants.ErrorMessageCompleteBasketFailed, actual.ErrorMessage?[0]);
        }

        #endregion

        #region Get Basket With Items (Admin)

        [Fact]
        public async Task GetBasketWithItemsAsync_WhenFound_ReturnsOk()
        {
            var basketDto = BasketTestData.GetSampleBasketDTO();
            var expected = ServiceResult<BasketDTO>.Success(basketDto);

            _basketServiceMock.Setup(s => s.GetBasketWithItemsAsync(basketDto.Id))
                              .ReturnsAsync(expected);

            var result = await _controller.GetBasketWithItems(basketDto.Id);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, objectResult.StatusCode);

            var actual = Assert.IsType<ServiceResult<BasketDTO>>(objectResult.Value);
            Assert.True(actual.IsSuccess);
            Assert.Equal(basketDto.Id, actual.Data?.Id);
        }

        [Fact]
        public async Task GetBasketWithItemsAsync_WhenNotFound_ReturnsBadRequest()
        {
            var basketId = TestConstants.SampleBasketId;
            var expected = ServiceResult<BasketDTO>.Fail(TestConstants.ErrorMessageBasketNotFound);

            _basketServiceMock.Setup(s => s.GetBasketWithItemsAsync(basketId))
                              .ReturnsAsync(expected);

            var result = await _controller.GetBasketWithItems(basketId);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, objectResult.StatusCode);

            var actual = Assert.IsType<ServiceResult<BasketDTO>>(objectResult.Value);
            Assert.False(actual.IsSuccess);
            Assert.Equal(TestConstants.ErrorMessageBasketNotFound, actual.ErrorMessage?[0]);
        }

        #endregion
    }
}
