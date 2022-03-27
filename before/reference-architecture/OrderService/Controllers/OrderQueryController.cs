using Microsoft.AspNetCore.Mvc;
using OrderService.Domain.OrderAggregate;
using OrderService.DTO.Read;
using OrderState = OrderService.DTO.Read.OrderState;

namespace OrderService.Controllers;

[Route("api/order")]
[ApiController]
public class OrderQueryController : ControllerBase
{
    // TODO: Add ctor
    
    // TODO: Complete actions

    private IEnumerable<OrderView> MapOrderViews(IEnumerable<Order?> orders) =>
        orders.Select(o => new OrderView
        {
            Id = o!.Id,
            CustomerId = o.CustomerId,
            OrderDate = o.OrderDate,
            OrderTotal = o.OrderItems.Sum(i => i.ProductPrice),
            Street = o.ShippingAddress.Street,
            City = o.ShippingAddress.City,
            State = o.ShippingAddress.State,
            Country = o.ShippingAddress.Country,
            PostalCode = o.ShippingAddress.PostalCode,
            OrderState = (OrderState)o.OrderState,
            ETag = o.ETag ?? string.Empty
        });
}