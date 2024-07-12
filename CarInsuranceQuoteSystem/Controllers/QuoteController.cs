using System;
using CarInsuranceQuoteSystem.DTO;
using CarInsuranceQuoteSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarInsuranceQuoteSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuoteController : ControllerBase
    {
        private readonly IQuoteService _quoteService;

        public QuoteController(IQuoteService quoteService)
        {
            _quoteService = quoteService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuote([FromBody] QuoteCreateDTO request)
        {
            var createdQuote = await _quoteService.CreateQuoteAsync(request);
            if (createdQuote == null)
                return BadRequest("Customer doesn't exist");
            return Ok(createdQuote);
        }

        [HttpGet("car/{carModel}")]
        public async Task<IActionResult> GetQuotesByCarModel(string carModel)
        {
            var quotes = await _quoteService.GetQuotesByCarModelAsync(carModel);
            if (quotes.Count() == 0)
                return NotFound("No cars available");
            return Ok(quotes);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuote(int id)
        {
            var deleted = await _quoteService.DeleteQuoteAsync(id);
            if (!deleted)
                return NotFound();
            return NoContent();
        }
    }
}

