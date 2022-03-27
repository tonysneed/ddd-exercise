using Microsoft.AspNetCore.Mvc;

namespace CustomerService.Controllers;

[Route("api/customer")]
[ApiController]
public class CustomerQueryController : ControllerBase
{
    // TODO: Complete actions
    
    // GET api/customer
    [HttpGet]
    public async Task<IActionResult> GetCustomers()
    {
        throw new NotImplementedException();
    }

    // GET api/customer/id
    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetCustomer([FromRoute] Guid id)
    {
        throw new NotImplementedException();
    }
}