using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OrderService.Domain.OrderAggregate;
using OrderService.Repositories;

namespace OrderService.Controllers;

[Route("api/order")]
[ApiController]
public class OrderCommandController : ControllerBase
{
    private readonly IOrderRepository _repository;
    private readonly IMapper _mapper;

    public OrderCommandController(
        IOrderRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    // POST api/order
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DTO.Write.Order orderDto)
    {
        var orderIn = _mapper.Map<Order>(orderDto);
        var result = await _repository.AddAsync(orderIn);

        var orderOut = _mapper.Map<DTO.Write.Order>(result);
        return Created($"api/order/{orderOut.Id}", orderOut);
    }

    // PUT api/order
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] DTO.Write.Order orderDto)
    {
        var orderIn = _mapper.Map<Order>(orderDto);
        var result = await _repository.UpdateAsync(orderIn);

        var orderOut = _mapper.Map<DTO.Write.Order>(result);
        return Ok(orderOut);
    }

    // DELETE api/order
    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> Remove([FromRoute] Guid id)
    {
        await _repository.RemoveAsync(id);
        return NoContent();
    }

    // PUT api/order/ship
    [HttpPut]
    [Route("ship/{id:guid}/{etag}")]
    public async Task<IActionResult> Ship([FromRoute] Guid id)
    {
        var order = await _repository.GetAsync(id);
        var result = await _repository.UpdateOrderStateAsync(order!, OrderState.Shipped);

        var orderOut = _mapper.Map<DTO.Write.Order>(result);
        return Ok(orderOut);
    }

    // PUT api/order/cancel
    [HttpPut]
    [Route("cancel/{id:guid}/{etag}")]
    public async Task<IActionResult> Cancel([FromRoute] Guid id)
    {
        var order = await _repository.GetAsync(id);
        var result = await _repository.UpdateOrderStateAsync(order!, OrderState.Cancelled);

        var orderOut = _mapper.Map<DTO.Write.Order>(result);
        return Ok(orderOut);
    }
}