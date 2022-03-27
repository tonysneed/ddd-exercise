using System;
using System.Threading.Tasks;
using AutoMapper;
using CustomerService.Controllers;
using CustomerService.Domain.CustomerAggregate;
using CustomerService.Repositories;
using CustomerService.Tests.Fakes;
using CustomerService.Tests.Helpers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CustomerService.Tests.Controllers;

public class CustomerCommandControllerTests
{
    private readonly Mock<ICustomerRepository> _repositoryMoq;
    private readonly IMapper _mapper;

    public CustomerCommandControllerTests()
    {
        _repositoryMoq = new Mock<ICustomerRepository>();
        _mapper = MappingHelper.GetMapper();
    }

    [Fact]
    public void WhenInstantiated_ThenShouldBeOfCorrectType()
    {
        var controller = new CustomerCommandController(_repositoryMoq.Object, _mapper);

        Assert.IsAssignableFrom<ControllerBase>(controller);
        Assert.IsType<CustomerCommandController>(controller);
    }

    [Fact]
    public async Task GivenWeAreCreatingACustomer_WhenSuccessful_ThenShouldProvideNewEntityWithPath()
    {
        var customerOut = _mapper.Map<Customer>(Customers.Customer1);

        _repositoryMoq.Setup(x => x.AddAsync(It.IsAny<Customer>()))
            .ReturnsAsync(customerOut);
    
        var controller = new CustomerCommandController(_repositoryMoq.Object, _mapper);

        var actionResult = await controller.Create(Customers.Customer1);
        var createdResult = actionResult as CreatedResult;

        Assert.NotNull(actionResult);
        Assert.NotNull(createdResult);
        Assert.Equal($"api/customer/{customerOut.Id}", createdResult!.Location, true);
    }

    [Fact]
    public async Task GivenWeAreUpdatingACustomer_WhenSuccessful_ThenUpdatedEntityShouldBeReturned()
    {
        var customerIn = Customers.Customer2;
        var customerOut = _mapper.Map<Customer>(Customers.Customer2);

        var controller = new CustomerCommandController(_repositoryMoq.Object, _mapper);

        _repositoryMoq.Setup(x => x.UpdateAsync(It.IsAny<Customer>()))
            .ReturnsAsync(customerOut);

        var actionResult = await controller.Update(customerIn);
        var objectResult = actionResult as OkObjectResult;

        Assert.NotNull(actionResult);
        Assert.NotNull(objectResult);
        Assert.Equal(customerIn.Id, ((DTO.Write.Customer)objectResult!.Value!).Id);
    }

    [Fact]
    public async Task GivenWeAreRemovingACustomer_WhenSuccessful_ThenShouldReturnSuccess()
    {
        var customerId = Guid.NewGuid();
        var controller = new CustomerCommandController(_repositoryMoq.Object, _mapper);

        _repositoryMoq.Setup(x => x.RemoveAsync(It.IsAny<Guid>()))
            .ReturnsAsync(1);

        var actionResult = await controller.Remove(customerId);
        var noContentResult = actionResult as NoContentResult;

        Assert.NotNull(actionResult);
        Assert.NotNull(noContentResult);
    }
}