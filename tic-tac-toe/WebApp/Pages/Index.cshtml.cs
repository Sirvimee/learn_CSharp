using GameBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class IndexModel : PageModel
{
    [BindProperty] public string? PlayerName { get; set; } 
    public string? Error { get; set; }

    public IActionResult OnPost()
    {
        if (string.IsNullOrEmpty(PlayerName))
        {
            ModelState.AddModelError(string.Empty, "Player name cannot be empty.");
            return Page();
        }
        
        return RedirectToPage("/Menu", new { playerName = PlayerName });
    }

}