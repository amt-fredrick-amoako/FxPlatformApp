using Entities;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
    public class BuyOrderRequest : IValidatableObject
    {
        [Required]
        public string? StockSymbol { get; set; } = string.Empty;
        [Required]
        public string? StockName { get; set; } = string.Empty;

        public DateTime DateAndTimeOfOrder { get; set; }

        [Range(1, 10000, ErrorMessage = "Value should be {0} minimum and {1} maximum")]
        public uint Quantity { get; set; }

        [Range(1, 10000, ErrorMessage = "Value should be {0} minimum and {1} maximum")]
        public double Price { get; set; }

        /// <summary>
        /// Non reusable model class validation using IValidationObject
        /// </summary>
        /// <param name="validationContext">ValidationContext to validate</param>
        /// <returns>validation errors as ValidationResult</returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            //Date of order should be less than Jan 01, 2000
            if (DateAndTimeOfOrder < Convert.ToDateTime("01-01-2000"))
                results.Add(new ValidationResult("Date of the order should not be older than Jan 01, 2000"));
            return results;
        }

        public BuyOrder ToBuyOrder()
        {
            return new BuyOrder
            {
                StockName = StockName,
                Price = Price,
                StockSymbol = StockSymbol,
                DateAndTimeOfOrder = DateAndTimeOfOrder,
                Quantity = Quantity,
                
            };
        }
    }
}
