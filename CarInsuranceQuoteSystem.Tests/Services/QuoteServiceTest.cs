using System;
using CarInsuranceQuoteSystem.Data;
using CarInsuranceQuoteSystem.DTO;
using CarInsuranceQuoteSystem.Models;
using CarInsuranceQuoteSystem.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CarInsuranceQuoteSystem.Tests.Services
{
	public class QuoteServiceTest

    {
        private readonly DbContextOptions<AppDbContext> _options;

        public QuoteServiceTest()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task CreateQuoteAsync_ShouldReturnQuote_WhenCustomerExists()
        {
            // Arrange
            await using var context = new AppDbContext(_options);
            var customer = new Customer { Id = 1 };
            context.Customers.Add(customer);
            await context.SaveChangesAsync();

            var request = new QuoteCreateDTO
            {
                CustomerId = 1,
                CarModel = "TestModel",
                CarYear = 2020,
                Price = 1000
            };

            var quoteService = new QuoteService(context);

            // Act
            var result = await quoteService.CreateQuoteAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(request.CustomerId, result.CustomerId);
            Assert.Equal(request.CarModel, result.CarModel);
            Assert.Equal(request.CarYear, result.CarYear);
            Assert.Equal(request.Price, result.Price);
        }

        [Fact]
        public async Task CreateQuoteAsync_ShouldReturnNull_WhenCustomerDoesNotExist()
        {
            // Arrange
            await using var context = new AppDbContext(_options);
            var request = new QuoteCreateDTO
            {
                CustomerId = 1,
                CarModel = "TestModel",
                CarYear = 2020,
                Price = 1000
            };

            var quoteService = new QuoteService(context);

            // Act
            var result = await quoteService.CreateQuoteAsync(request);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetQuotesByCarModelAsync_ShouldReturnQuotes_WhenQuotesExist()
        {
            // Arrange
            await using var context = new AppDbContext(_options);
            var carModel = "TestModel";
            var quotes = new List<Quote>
            {
                new Quote { CustomerId = 1, CarModel = carModel, CarYear = 2020, Price = 1000 },
                new Quote { CustomerId = 2, CarModel = carModel, CarYear = 2021, Price = 1200 }
            };
            context.Quotes.AddRange(quotes);
            await context.SaveChangesAsync();

            var quoteService = new QuoteService(context);

            // Act
            var result = await quoteService.GetQuotesByCarModelAsync(carModel);

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetQuotesByCarModelAsync_ShouldReturnEmpty_WhenNoQuotesExist()
        {
            // Arrange
            await using var context = new AppDbContext(_options);
            var carModel = "NonExistentModel";

            var quoteService = new QuoteService(context);

            // Act
            var result = await quoteService.GetQuotesByCarModelAsync(carModel);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task DeleteQuoteAsync_ShouldReturnTrue_WhenQuoteExists()
        {
            // Arrange
            await using var context = new AppDbContext(_options);
            var quote = new Quote { Id = 1, CustomerId = 1, CarModel = "TestModel", CarYear = 2020, Price = 1000 };
            context.Quotes.Add(quote);
            await context.SaveChangesAsync();

            var quoteService = new QuoteService(context);

            // Act
            var result = await quoteService.DeleteQuoteAsync(1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteQuoteAsync_ShouldReturnFalse_WhenQuoteDoesNotExist()
        {
            // Arrange
            await using var context = new AppDbContext(_options);
            var quoteService = new QuoteService(context);

            // Act
            var result = await quoteService.DeleteQuoteAsync(1);

            // Assert
            Assert.False(result);
        }
    }
}

