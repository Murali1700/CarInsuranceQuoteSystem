using System;
using CarInsuranceQuoteSystem.Data;
using CarInsuranceQuoteSystem.DTO;
using CarInsuranceQuoteSystem.Models;
using CarInsuranceQuoteSystem.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace CarInsuranceQuoteSystem.Tests.Services
{
    public class CustomerServiceTest
    {
        private readonly DbContextOptions<AppDbContext> _options;

        public CustomerServiceTest()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task CreateCustomerAsync_ShouldCreateNewCustomer()
        {
            // Arrange
            await using var context = new AppDbContext(_options);
            var customerService = new CustomerService(context);
            var customerDto = new CustomerCreateDTO
            {
                FirstName = "John",
                LastName = "Doe",
                Address = "123 Main St",
                DateOfBirth = new DateTime(1980, 1, 1)
            };

            // Act
            var customer = await customerService.CreateCustomerAsync(customerDto);

            // Assert
            Assert.NotNull(customer);
            Assert.Equal(customerDto.FirstName, customer.FirstName);
            Assert.Equal(customerDto.LastName, customer.LastName);
            Assert.Equal(customerDto.Address, customer.Address);
            Assert.Equal(customerDto.DateOfBirth, customer.DateOfBirth);
        }

        [Fact]
        public async Task UpdateCustomerAsync_ShouldReturnNullIfCustomerNotFound()
        {
            // Arrange
            await using var context = new AppDbContext(_options);
            var customerService = new CustomerService(context);
            var customerDto = new CustomerCreateDTO
            {
                FirstName = "Jane",
                LastName = "Smith",
                Address = "456 Elm St",
                DateOfBirth = new DateTime(1990, 1, 1)
            };

            // Act
            var updatedCustomer = await customerService.UpdateCustomerAsync(1, customerDto);

            // Assert
            Assert.Null(updatedCustomer);
        }

        [Fact]
        public async Task UpdateCustomerAsync_ShouldUpdateCustomerIfFound()
        {
            // Arrange
            await using var context = new AppDbContext(_options);
            var customerService = new CustomerService(context);
            var customer = new Customer
            {
                FirstName = "OldFirstName",
                LastName = "OldLastName",
                Address = "OldAddress",
                DateOfBirth = new DateTime(1980, 1, 1)
            };
            context.Customers.Add(customer);
            await context.SaveChangesAsync();

            var customerDto = new CustomerCreateDTO
            {
                FirstName = "NewFirstName",
                LastName = "NewLastName",
                Address = "NewAddress",
                DateOfBirth = new DateTime(1980, 1, 1)
            };

            // Act
            var updatedCustomer = await customerService.UpdateCustomerAsync(customer.Id, customerDto);

            // Assert
            Assert.NotNull(updatedCustomer);
            Assert.Equal(customerDto.FirstName, updatedCustomer.FirstName);
            Assert.Equal(customerDto.LastName, updatedCustomer.LastName);
            Assert.Equal(customerDto.Address, updatedCustomer.Address);
        }

        [Fact]
        public async Task GetCustomersWithQuotesOverAmountAsync_ShouldReturnCustomersWithQuotesOverAmount()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            await using (var context = new AppDbContext(options))
            {
                var customer = new Customer
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Address = "123 Main St",
                    DateOfBirth = new DateTime(1980, 1, 1),
                    Quotes = new List<Quote>
                    {
                        new Quote { CarModel = "Toyota", CarYear = 2020, Price = 15000 },
                        new Quote { CarModel = "Honda", CarYear = 2019, Price = 5000 }
                    }
                };

                context.Customers.Add(customer);
                await context.SaveChangesAsync();
            }

            await using (var context = new AppDbContext(options))
            {
                var customerService = new CustomerService(context);

                // Act
                var customers = await customerService.GetCustomersWithQuotesOverAmountAsync(10000);

                // Assert
                Assert.Single(customers);
                var resultCustomer = customers.First();
                Assert.Equal("John", resultCustomer.FirstName);
                Assert.Single(resultCustomer.Quotes);
                Assert.Equal(15000, resultCustomer.Quotes.First().Price);
            }
        }
    }
}

