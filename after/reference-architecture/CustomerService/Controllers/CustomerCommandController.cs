using AutoMapper;
using CustomerService.Domain.CustomerAggregate;
using CustomerService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CustomerService.Controllers;

[Route("api/customer")]
[ApiController]
public class CustomerCommandController : ControllerBase
{
    private readonly ICustomerRepository _repository;
    private readonly IMapper _mapper;

    public CustomerCommandController(
        ICustomerRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    // POST api/customer
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DTO.Write.Customer customerDto)
    {
        var customerIn = _mapper.Map<Customer>(customerDto);
        var result = await _repository.AddAsync(customerIn);

        var customerOut = _mapper.Map<DTO.Write.Customer>(result);
        return Created($"api/customer/{customerOut.Id}", customerOut);
    }

    // PUT api/customer
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] DTO.Write.Customer customerDto)
    {
        var customerIn = _mapper.Map<Customer>(customerDto);
        var result = await _repository.UpdateAsync(customerIn);

        var customerOut = _mapper.Map<DTO.Write.Customer>(result);
        return Ok(customerOut);
    }

    // DELETE api/customer/id
    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Remove([FromRoute] Guid id)
    {
        await _repository.RemoveAsync(id);
        return NoContent();
    }
}