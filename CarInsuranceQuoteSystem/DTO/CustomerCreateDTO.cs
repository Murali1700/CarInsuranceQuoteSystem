using System;
using System.ComponentModel.DataAnnotations;

namespace CarInsuranceQuoteSystem.DTO
{
	public class CustomerCreateDTO
	{
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        [StringLength(100)]
        public string Address { get; set; } = string.Empty;
    }
}

