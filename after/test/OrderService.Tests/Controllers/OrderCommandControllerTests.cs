using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OrderService.Controllers;
using OrderService.Domain.OrderAggregate;
using OrderService.Repositories;
using OrderService.Tests.Fakes;
using OrderService.Tests.Helpers;
using Xunit;

namespace OrderService.Tests.Controllers;

public class OrderCommandControllerTests
{
    private readonly Mock<IOrderRepository> _repositoryMoq;
    private readonly IMapper _mapper;

    public OrderCommandControllerTests()
    {
        _repositoryMoq = new Mock<IOrderRepository>();
        _mapper = MappingHelper.GetMapper();
    }

    [Fact]
    public void WhenInstantiated_ThenShouldBeOfCorrectType()
    {
        var controller = new OrderCommandController(_repositoryMoq.Object, _mapper);

        Assert.NotNull(controller);
        Assert.IsAssignableFrom<ControllerBase>(controller);
        Assert.IsType<OrderCommandController>(controller);
    }

    [Fact]
    public async Task GivenWeAreCreatingAnOrder_WhenSuccessful_ThenShouldProvideNewEntityWithPath()
    {
        var orderOut = _mapper.Map<Order>(Orders.Order1);
        _repositoryMoq.Setup(x => x.AddAsync(It.IsAny<Order>()))
            .ReturnsAsync(orderOut);
        var controller = new OrderCommandController(_repositoryMoq.Object, _mapper);

        var actionResult = await controller.Create(Orders.Order1);
        var createdResult = actionResult as CreatedResult;

        Assert.NotNull(actionResult);
        Assert.NotNull(createdResult);
        Assert.Equal($"api/order/{orderOut.Id}", createdResult!.Location, true);
    }

    [Fact]
    public async Task GivenWeAreUpdatingAnOrder_WhenSuccessful_ThenUpdatedEntityShouldBeReturned()
    {
        var orderIn = Orders.Order1;
        var orderOut = _mapper.Map<Order>(orderIn);
        var controller = new OrderCommandController(_repositoryMoq.Object, _mapper);
        _repositoryMoq.Setup(x => x.UpdateAsync(It.IsAny<Order>()))
            .ReturnsAsync(orderOut);

        var actionResult = await controller.Update(orderIn);
        var okResult = Assert.IsType<OkObjectResult>(actionResult);

        Assert.NotNull(actionResult);
        Assert.NotNull(okResult);
        Assert.Equal(orderIn.Id, ((DTO.Write.Order) okResult.Value!).Id);
    }

    [Fact]
    public async Task GivenWeAreRemovingAnOrder_WhenSuccessful_ThenShouldReturnSuccess()
    {
        var orderId = Guid.NewGuid();
        _repositoryMoq.Setup(x => x.RemoveAsync(It.IsAny<Guid>()))
            .ReturnsAsync(1);
        var controller = new OrderCommandController(_repositoryMoq.Object, _mapper);

        var actionResult = await controller.Remove(orderId);
        var noContentResult = actionResult as NoContentResult;

        Assert.NotNull(actionResult);
        Assert.NotNull(noContentResult);
    }

    [Fact]
    public async Task GivenWeAreShippingAnOrder_WhenSuccessful_ThenShouldReturnEntity()
    {
        var orderIn = Orders.Order1;
        var orderOut = _mapper.Map<Order>(Orders.Order1);
        var controller = new OrderCommandController(_repositoryMoq.Object, _mapper);
        _repositoryMoq.Setup(x => x.UpdateOrderStateAsync(It.IsAny<Order>(), OrderState.Shipped))
            .ReturnsAsync(orderOut);

        var actionResult = await controller.Ship(orderIn.Id);
        var okResult = Assert.IsType<OkObjectResult>(actionResult);

        Assert.NotNull(actionResult);
        Assert.NotNull(okResult);
        Assert.Equal(orderOut.Id, ((DTO.Write.Order) okResult.Value!).Id);
    }

    [Fact]
    public async Task GivenWeAreCancellingAnOrder_WhenSuccessful_ThenShouldReturnEntity()
    {
        var orderIn = Orders.Order2;
        var orderOut = _mapper.Map<Order>(Orders.Order2);
        var controller = new OrderCommandController(_repositoryMoq.Object, _mapper);
        _repositoryMoq.Setup(x => x.UpdateOrderStateAsync(It.IsAny<Order>(), OrderState.Cancelled))
            .ReturnsAsync(orderOut);

        var actionResult = await controller.Cancel(orderIn.Id);
        var okResult = Assert.IsType<OkObjectResult>(actionResult);

        Assert.NotNull(actionResult);
        Assert.NotNull(okResult);
        Assert.Equal(orderOut.Id, ((DTO.Write.Order) okResult.Value!).Id);
    }
}