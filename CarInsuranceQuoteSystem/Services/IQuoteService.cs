using System;
using CarInsuranceQuoteSystem.DTO;
using CarInsuranceQuoteSystem.Models;

namespace CarInsuranceQuoteSystem.Services
{
	public interface IQuoteService
    {
        Task<Quote?> CreateQuoteAsync(QuoteCreateDTO quote);
        Task<IEnumerable<Quote>> GetQuotesByCarModelAsync(string carModel);
        Task<bool> DeleteQuoteAsync(int quoteId);
    }
}

