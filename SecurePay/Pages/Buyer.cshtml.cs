using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SecurePay.Models;

namespace SecurePay.Pages
{
    public class BuyerModel : PageModel
    {
        [BindProperty]
        public BuyerInfo Buyer { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                // Chuyển hướng đến trang payment với các thông tin của buyer
                return RedirectToPage("/Payment", new { Buyer.Name, Buyer.Email, Buyer.Address, Buyer.Amount });
            }

            return Page();
        }
    }
}
