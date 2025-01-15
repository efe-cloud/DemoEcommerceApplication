using System.ComponentModel.DataAnnotations;

namespace Ecommerce.client.Models
{
    public class PaymentDetails
    {
        [Required(ErrorMessage = "Card number is required.")]
        [CreditCard(ErrorMessage = "Invalid card number.")]
        [RegularExpression(@"^(4|5)\d{15}$", ErrorMessage = "Card number must start with 4 (Visa) or 5 (MasterCard) and be 16 digits.")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "CCV is required.")]
        [RegularExpression(@"^\d{3,4}$", ErrorMessage = "CCV must be 3 or 4 digits.")]
        public string Ccv { get; set; }

        [Required(ErrorMessage = "Expiration month is required.")]
        [Range(1, 12, ErrorMessage = "Month must be between 1 and 12.")]
        public int ExpirationMonth { get; set; }

        [Required(ErrorMessage = "Expiration year is required.")]
        [Range(2023, 2100, ErrorMessage = "Year must be a valid future year.")]
        public int ExpirationYear { get; set; }

        [Required(ErrorMessage = "Billing address is required.")]
        public string BillingAddress { get; set; }

        [Required(ErrorMessage = "Please select a payment option.")]
        public string PaymentOption { get; set; } // e.g., Credit Card, PayPal, Bank Transfer
    }
}
