using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using CourseTech.Core.DTOs.Basket;
using CourseTech.Core.Models;
using CourseTech.Core.Repositories;
using CourseTech.Core.Services;
using CourseTech.Core.UnitOfWorks;
using CourseTech.Service.Services;
using CourseTech.Shared;
using Moq;
using NUnit.Framework;

namespace CourseTech.Tests.Services
{
    [TestFixture]
    public class BasketServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IBasketRepository> _basketRepoMock;
        private Mock<ICourseRepository> _courseRepoMock;
        private Mock<IMapper> _mapperMock;
        private IBasketService _basketService;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _basketRepoMock = new Mock<IBasketRepository>();
            _courseRepoMock = new Mock<ICourseRepository>();
            _mapperMock = new Mock<IMapper>();

            _unitOfWorkMock.Setup(u => u.Basket).Returns(_basketRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.Course).Returns(_courseRepoMock.Object);

            _basketService = new BasketService(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task GetActiveBasketAsync_ReturnsBasket_WhenExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var basket = new Basket(userId);
            var basketDto = new BasketDTO(basket.Id, userId, new List<BasketItemDTO>(), "Active", 0);

            _basketRepoMock.Setup(r => r.GetBasketByUserIdAsync(userId)).ReturnsAsync(basket);
            _mapperMock.Setup(m => m.Map<BasketDTO>(basket)).Returns(basketDto);

            // Act
            var result = await _basketService.GetActiveBasketAsync(userId);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(basketDto, result.Data);
        }

        [Test]
        public async Task AddCourseToBasketAsync_ReturnsFail_WhenCourseNotFound()
        {
            var userId = Guid.NewGuid();
            var courseId = Guid.NewGuid();

            _courseRepoMock.Setup(c => c.GetByIdAsync(courseId)).ReturnsAsync((Course)null);

            var result = await _basketService.AddCourseToBasketAsync(userId, courseId);

            Assert.IsTrue(result.IsFail);
            Assert.Contains("Course not found", result.ErrorMessage);
        }

        [Test]
        public async Task RemoveCourseFromBasketAsync_ReturnsSuccess_WhenCourseRemoved()
        {
            var userId = Guid.NewGuid();
            var courseId = Guid.NewGuid();
            var basket = new Basket(userId);
            var course = new Course("title", "desc", "img", "vid", 0, 0, 10, TimeSpan.FromHours(1), Guid.NewGuid(), Guid.NewGuid());

            basket.AddCourse(course);
            _basketRepoMock.Setup(b => b.GetBasketByUserIdAsync(userId)).ReturnsAsync(basket);

            var result = await _basketService.RemoveCourseFromBasketAsync(userId, course.Id);

            Assert.IsTrue(result.IsSuccess);
        }

        [Test]
        public async Task ClearBasketAsync_ReturnsSuccess_WhenBasketCleared()
        {
            var userId = Guid.NewGuid();
            var basket = new Basket(userId);
            var course = new Course("title", "desc", "img", "vid", 0, 0, 10, TimeSpan.FromHours(1), Guid.NewGuid(), Guid.NewGuid());
            basket.AddCourse(course);

            _basketRepoMock.Setup(b => b.GetBasketByUserIdAsync(userId)).ReturnsAsync(basket);

            var result = await _basketService.ClearBasketAsync(userId);

            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(0, basket.BasketItems.Count);
        }

        [Test]
        public async Task CompleteBasketAsync_ReturnsFail_WhenEmpty()
        {
            var userId = Guid.NewGuid();
            var basket = new Basket(userId);

            _basketRepoMock.Setup(b => b.GetBasketByUserIdAsync(userId)).ReturnsAsync(basket);

            Assert.ThrowsAsync<InvalidOperationException>(() => _basketService.CompleteBasketAsync(userId));
        }

        [Test]
        public async Task GetBasketWithItemsAsync_ReturnsSuccess_WhenBasketFound()
        {
            var basketId = Guid.NewGuid();
            var basket = new Basket(Guid.NewGuid());
            var dto = new BasketDTO(basketId, basket.UserId, new List<BasketItemDTO>(), "Active", 200);

            _basketRepoMock.Setup(b => b.GetBasketWithItemsAsync(basketId)).ReturnsAsync(basket);
            _mapperMock.Setup(m => m.Map<BasketDTO>(basket)).Returns(dto);

            var result = await _basketService.GetBasketWithItemsAsync(basketId);

            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(dto, result.Data);
        }

        [Test]
        public async Task GetBasketWithItemsAsync_ReturnsFail_WhenBasketNotFound()
        {
            var basketId = Guid.NewGuid();
            _basketRepoMock.Setup(b => b.GetBasketWithItemsAsync(basketId)).ReturnsAsync((Basket)null);

            var result = await _basketService.GetBasketWithItemsAsync(basketId);

            Assert.IsTrue(result.IsFailure);
            Assert.Contains("Basket not found.", result.ErrorMessage);
        }
    }
}
