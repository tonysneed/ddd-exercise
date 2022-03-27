using Microsoft.AspNetCore.Mvc;

namespace CustomerService.Controllers;

[Route("api/customer")]
[ApiController]
public class CustomerCommandController : ControllerBase
{
    // TODO: Add ctor
    
    // TODO: Complete actions
    
    // POST api/customer
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DTO.Write.Customer customerDto)
    {
        throw new NotImplementedException();
    }

    // PUT api/customer
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] DTO.Write.Customer customerDto)
    {
        throw new NotImplementedException();
    }

    // DELETE api/customer/id
    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Remove([FromRoute] Guid id)
    {
        throw new NotImplementedException();
    }
}