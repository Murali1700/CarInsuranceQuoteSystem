using System;
using CarInsuranceQuoteSystem.DTO;
using CarInsuranceQuoteSystem.Models;

namespace CarInsuranceQuoteSystem.Services
{
	public interface ICustomerService
    {
        Task<Customer> CreateCustomerAsync(CustomerCreateDTO customer);
        Task<Customer?> UpdateCustomerAsync(int id, CustomerCreateDTO customer);
        Task<IEnumerable<Customer>> GetCustomersWithQuotesOverAmountAsync(decimal amount);
    }
}

