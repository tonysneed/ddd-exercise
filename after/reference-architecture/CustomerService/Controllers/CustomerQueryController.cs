﻿using AutoMapper;
using CustomerService.DTO.Read;
using CustomerService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CustomerService.Controllers;

[Route("api/customer")]
[ApiController]
public class CustomerQueryController : ControllerBase
{
    private readonly ICustomerRepository _repository;
    private readonly IMapper _mapper;

    public CustomerQueryController(
        ICustomerRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    // GET api/customer
    [HttpGet]
    public async Task<IActionResult> GetCustomers()
    {
        var customers = await _repository.GetAsync();
        var result = _mapper.Map<IEnumerable<CustomerView>>(customers);
        return Ok(result);
    }

    // GET api/customer/id
    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetCustomer([FromRoute] Guid id)
    {
        var customer = await _repository.GetAsync(id);
        if (customer == null) return NotFound();
        var result = _mapper.Map<CustomerView>(customer);
        return Ok(result);
    }
}