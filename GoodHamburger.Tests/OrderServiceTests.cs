using Moq;
using FluentAssertions;
using Good_Hamburger.Models;
using Good_Hamburger.Enums;
using Good_Hamburger.Repository;
using Good_Hamburger.Services;
using Good_Hamburger.Services.Discounts;
using Xunit;

namespace GoodHamburger.Tests
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly Mock<IFoodRepository> _foodRepositoryMock;
        private readonly List<IDiscountRule> _rules;
        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _foodRepositoryMock = new Mock<IFoodRepository>();

            // Usamos as regras REAIS para testar a integração com o motor de descontos
            _rules = new List<IDiscountRule>
            {
                new ComboCompletoRule(),
                new SandwichDrinkRule(),
                new SandwichSideRule()
            };

            _orderService = new OrderService(
                _orderRepositoryMock.Object, 
                _foodRepositoryMock.Object, 
                _rules);
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldThrowException_WhenDuplicateItemTypeIsProvided()
        {
            // Arrange
            var foodIds = new List<int> { 1, 2 }; // Dois sanduíches
            var food1 = new Food { Id = 1, Name = "X Burger", TypeFood = FoodType.SANDUICHE, Price = 5.00m };
            var food2 = new Food { Id = 2, Name = "X Bacon", TypeFood = FoodType.SANDUICHE, Price = 7.00m };

            _foodRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(food1);
            _foodRepositoryMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(food2);

            // Act
            Func<Task> act = async () => await _orderService.CreateOrderAsync(foodIds);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("*contém itens duplicados*");
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldApply20PercentDiscount_ForCompleteCombo()
        {
            // Arrange
            var foodIds = new List<int> { 1, 4, 5 }; // Sanduíche + Acompanhamento + Bebida
            var burger = new Food { Id = 1, Price = 10.00m, TypeFood = FoodType.SANDUICHE };
            var side = new Food { Id = 4, Price = 5.00m, TypeFood = FoodType.ACOMPANHAMENTO };
            var drink = new Food { Id = 5, Price = 5.00m, TypeFood = FoodType.BEBIDA };

            _foodRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(burger);
            _foodRepositoryMock.Setup(r => r.GetByIdAsync(4)).ReturnsAsync(side);
            _foodRepositoryMock.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(drink);

            _orderRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<Order>()))
                               .ReturnsAsync((Order o) => o);

            // Act
            var result = await _orderService.CreateOrderAsync(foodIds);

            // Assert
            result.Subtotal.Should().Be(20.00m);
            result.DiscountAmount.Should().Be(4.00m); // 20% de 20
            result.Total.Should().Be(16.00m);
            result.DiscountDescription.Should().Contain("20%");
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldApply15PercentDiscount_ForSandwichAndDrink()
        {
            // Arrange
            var foodIds = new List<int> { 1, 5 }; // Sanduíche + Bebida
            var burger = new Food { Id = 1, Price = 10.00m, TypeFood = FoodType.SANDUICHE };
            var drink = new Food { Id = 5, Price = 10.00m, TypeFood = FoodType.BEBIDA };

            _foodRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(burger);
            _foodRepositoryMock.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(drink);

            _orderRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<Order>()))
                               .ReturnsAsync((Order o) => o);

            // Act
            var result = await _orderService.CreateOrderAsync(foodIds);

            // Assert
            result.Subtotal.Should().Be(20.00m);
            result.DiscountAmount.Should().Be(3.00m); // 15% de 20
            result.Total.Should().Be(17.00m);
            result.DiscountDescription.Should().Contain("15%");
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldApply10PercentDiscount_ForSandwichAndSide()
        {
            // Arrange
            var foodIds = new List<int> { 1, 4 }; // Sanduíche + Acompanhamento
            var burger = new Food { Id = 1, Price = 10.00m, TypeFood = FoodType.SANDUICHE };
            var side = new Food { Id = 4, Price = 10.00m, TypeFood = FoodType.ACOMPANHAMENTO };

            _foodRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(burger);
            _foodRepositoryMock.Setup(r => r.GetByIdAsync(4)).ReturnsAsync(side);

            _orderRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<Order>()))
                               .ReturnsAsync((Order o) => o);

            // Act
            var result = await _orderService.CreateOrderAsync(foodIds);

            // Assert
            result.Subtotal.Should().Be(20.00m);
            result.DiscountAmount.Should().Be(2.00m); // 10% de 20
            result.Total.Should().Be(18.00m);
            result.DiscountDescription.Should().Contain("10%");
        }
    }
}
