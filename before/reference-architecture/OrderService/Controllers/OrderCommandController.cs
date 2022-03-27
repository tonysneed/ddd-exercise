using Microsoft.AspNetCore.Mvc;

namespace OrderService.Controllers;

[Route("api/order")]
[ApiController]
public class OrderCommandController : ControllerBase
{
    // TODO: Add ctor
    
    // TODO: Complete actions
    
    // POST api/order
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DTO.Write.Order orderDto)
    {
        throw new NotImplementedException();
    }

    // PUT api/order
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] DTO.Write.Order orderDto)
    {
        throw new NotImplementedException();
    }

    // DELETE api/order
    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> Remove([FromRoute] Guid id)
    {
        throw new NotImplementedException();
    }

    // PUT api/order/ship
    [HttpPut]
    [Route("ship/{id:guid}/{etag}")]
    public async Task<IActionResult> Ship([FromRoute] Guid id)
    {
        throw new NotImplementedException();
    }

    // PUT api/order/cancel
    [HttpPut]
    [Route("cancel/{id:guid}/{etag}")]
    public async Task<IActionResult> Cancel([FromRoute] Guid id)
    {
        throw new NotImplementedException();
    }
}