using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SecurePay.Pages
{
    public class PaymentModel : PageModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public decimal Amount { get; set; }

        public void OnGet(string name, string email, string address, decimal amount)
        {
            Name = name;
            Email = email;
            Address = address;
            Amount = amount;
        }
    }
}
