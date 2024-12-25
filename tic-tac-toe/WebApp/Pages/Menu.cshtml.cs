using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class Menu : PageModel
{
    [BindProperty(SupportsGet = true)] 
    public string PlayerName { get; set; } = "Default";
    
    public IActionResult OnGet()
    {
        if (string.IsNullOrWhiteSpace(PlayerName))
        {
            return RedirectToPage("/Index");
        }
        return Page();
    }
}