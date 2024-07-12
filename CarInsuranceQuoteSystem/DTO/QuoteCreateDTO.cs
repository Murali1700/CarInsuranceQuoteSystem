using System;
using System.ComponentModel.DataAnnotations;

namespace CarInsuranceQuoteSystem.DTO
{
	public class QuoteCreateDTO
	{
        [Required]
        [StringLength(50)]
        public string CarModel { get; set; } = string.Empty;
        [Required]
        [Range(1886, 2024, ErrorMessage = "Please enter a valid year: 1886 - 2024")]
        public int CarYear { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int CustomerId { get; set; }
    }
}

