using System;
using CarInsuranceQuoteSystem.DTO;
using CarInsuranceQuoteSystem.Models;
using CarInsuranceQuoteSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarInsuranceQuoteSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateCustomer([FromBody] CustomerCreateDTO request)
        {
            if(request != null)
            {
                var createdCustomer = await _customerService.CreateCustomerAsync(request);
                return Ok(createdCustomer);
            }
            return BadRequest("Improper Request Fields");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] CustomerCreateDTO request)
        {
            var updatedCustomer = await _customerService.UpdateCustomerAsync(id, request);
            if (updatedCustomer == null)
                return NotFound("Customer not found");
            return Ok(updatedCustomer);
        }
        
        [HttpGet("quotes/over/{amount}")]
        public async Task<ActionResult<List<Customer>>> GetCustomersWithQuotesOverAmount(decimal amount)
        {
            var customers = await _customerService.GetCustomersWithQuotesOverAmountAsync(amount);
            return Ok(customers);
        }
    }
}

