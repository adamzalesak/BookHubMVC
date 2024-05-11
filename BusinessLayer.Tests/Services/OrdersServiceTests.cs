using BusinessLayer.Models.Cart;
using BusinessLayer.Models.Order;
using BusinessLayer.Services;
using DataAccessLayer.Models;
using TestUtilities.MockedObjects;

namespace BusinessLayer.Tests.Services;

public class OrdersServiceTests
{
    private readonly OrdersService _ordersService;

    public OrdersServiceTests()
    {
        var dbContextOptions = MockedDbContext.GenerateNewInMemoryDbContextOptions();
        var dbContext = MockedDbContext.CreateFromOptions(dbContextOptions);
        
        _ordersService = new OrdersService(dbContext);
    }

    [Fact]
    public async Task CreateOrder_ValidInput_ReturnsOrderModel()
    {
        // Arrange
        var orderDto = new CreateOrderModel
        {
            UserId = "1",
            CartId = 2,
            Address = "Test Address",
            Email = "test@email.com",
            Phone = 420605123456,
            TotalPrice = 123.45m,
        };

        // Act
        var result = await _ordersService.CreateOrder(orderDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(orderDto.UserId, result.UserId);
        Assert.Equal(orderDto.CartId, result.CartId);
        Assert.Equal(orderDto.Address, result.Address);
        Assert.Equal(orderDto.Email, result.Email);
        Assert.Equal(orderDto.Phone, result.Phone);
        Assert.Equal(OrderState.Created, result.State);
        Assert.Equal(orderDto.TotalPrice, result.TotalPrice);
    }

    [Fact]
    public async Task CreateOrder_InvalidInput_ReturnsNull()
    {
        // Arrange
        var orderDto = new CreateOrderModel
        {
            Email = "test@test.com",
        };

        // Act
        var result = await _ordersService.CreateOrder(orderDto);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteOrder_ExistingOrder_ReturnsTrue()
    {
        // Arrange
        var orderId = 1;

        // Act
        var result = await _ordersService.DeleteOrder(orderId);
        var deletedOrder = await _ordersService.GetOrder(orderId);

        // Assert
        Assert.True(result);
        Assert.Null(deletedOrder);
    }

    [Fact]
    public async Task DeleteOrder_NonExistingOrder_ReturnsFalse()
    {
        // Arrange
        var orderId = 100;

        // Act
        var result = await _ordersService.DeleteOrder(orderId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task EditOrder_ValidInput_ReturnsTrue()
    {
        // Arrange
        var orderId = 1;
        var updateModel = new EditOrderModel()
        {
            Address = "New Address",
            Email = "new.email@email.com",
            Phone = 420605654321,
        };

        // Act
        var result = await _ordersService.EditOrder(orderId, updateModel);
        var editedOrder = await _ordersService.GetOrder(orderId);

        // Assert
        Assert.True(result);
        Assert.NotNull(editedOrder);
        Assert.Equal(updateModel.Address, editedOrder.Address);
        Assert.Equal(updateModel.Email, editedOrder.Email);
        Assert.Equal(updateModel.Phone, editedOrder.Phone);
    }

    [Fact]
    public async Task GetOrdersInInterval_ValidInput_ReturnsOrderModelList()
    {
        // Arrange
        var from = DateTime.Now.AddDays(-7);
        var to = DateTime.Now;

        // Act
        var result = await _ordersService.GetOrdersInInterval(from, to);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetAllOrders_ReturnsOrderModelList()
    {
        // Act
        var result = await _ordersService.GetAllOrders();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task GetOrder_ExistingOrder_ReturnsOrderModel()
    {
        // Arrange
        var orderId = 1;

        // Act
        var result = await _ordersService.GetOrder(orderId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(orderId, result.Id);
        Assert.Equal("1", result.UserId);
        Assert.Equal(1, result.CartId);
    }

    [Fact]
    public async Task GetOrder_NonExistingOrder_ReturnsNull()
    {
        // Arrange
        var orderId = 100;

        // Act
        var result = await _ordersService.GetOrder(orderId);

        // Assert
        Assert.Null(result);
    }
}