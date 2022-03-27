using EventDriven.DDD.Abstractions.Entities;

namespace CustomerService.Domain.CustomerAggregate;

public class Customer : 
    Entity
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public Address ShippingAddress { get; set; } = null!;
}