using System;
using CarInsuranceQuoteSystem.Data;
using CarInsuranceQuoteSystem.DTO;
using CarInsuranceQuoteSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CarInsuranceQuoteSystem.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly AppDbContext _context;

        public CustomerService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Customer> CreateCustomerAsync(CustomerCreateDTO request)
        {
            Customer customer = new Customer
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Address = request.Address,
                DateOfBirth = request.DateOfBirth
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<Customer?> UpdateCustomerAsync(int id, CustomerCreateDTO request)
        {
            Customer? customer = await _context.Customers.FindAsync(id);
            if(customer != null)
            {
                customer.Id = id;
                customer.FirstName = request.FirstName;
                customer.LastName = request.LastName;
                customer.Address = request.Address;
                customer.DateOfBirth = request.DateOfBirth;
                _context.Customers.Update(customer);
                await _context.SaveChangesAsync();
                return customer;
            }
            return null;

        }

        public async Task<IEnumerable<Customer>> GetCustomersWithQuotesOverAmountAsync(decimal amount)
        {
            
            return await _context.Customers
                .Where(c => c.Quotes.Any(q => q.Price > amount))
                .Select(c => new Customer
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    DateOfBirth = c.DateOfBirth,
                    Address = c.Address,
                    Quotes = c.Quotes.Where(q => q.Price > amount).ToList()
                })
                .ToListAsync();
        }
    }
}

