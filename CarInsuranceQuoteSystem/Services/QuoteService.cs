using System;
using CarInsuranceQuoteSystem.Data;
using CarInsuranceQuoteSystem.DTO;
using CarInsuranceQuoteSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CarInsuranceQuoteSystem.Services
{
    public class QuoteService : IQuoteService
    {
        private readonly AppDbContext _context;

        public QuoteService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Quote?> CreateQuoteAsync(QuoteCreateDTO request)
        {
            var customer =  _context.Customers
                                .Where(c => c.Id == request.CustomerId)
                                .FirstOrDefault();
            if(customer != null)
            {
                Quote quote = new Quote
                {
                    CustomerId = request.CustomerId,
                    CarModel = request.CarModel,
                    CarYear = request.CarYear,
                    Price = request.Price
                };
                _context.Quotes.Add(quote);
                await _context.SaveChangesAsync();
                return quote;
            }
            return null;
        }

        public async Task<IEnumerable<Quote>> GetQuotesByCarModelAsync(string carModel)
        {
            return await _context.Quotes
                .Where(q => q.CarModel.ToLower() == carModel.ToLower())
                .ToListAsync();
        }

        public async Task<bool> DeleteQuoteAsync(int quoteId)
        {
            var quote = await _context.Quotes.FindAsync(quoteId);
            if (quote == null)
                return false;

            _context.Quotes.Remove(quote);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

