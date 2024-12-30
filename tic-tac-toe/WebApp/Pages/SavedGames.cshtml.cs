using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace WebApp.Pages;

public class SavedGames : PageModel
{
    [BindProperty(SupportsGet = true)] public string PlayerName { get; set; } = "Default";
    [BindProperty] public string GameName { get; set; } = "Default";

    private readonly GameRepositoryDb _gameRepository;

    public List<string> SavedGamesList { get; set; } = new List<string>();
    
    public SavedGames(GameRepositoryDb gameRepository)
    {
        _gameRepository = gameRepository;
    }

    public IActionResult OnGet()
    {
        if (string.IsNullOrWhiteSpace(PlayerName))
        {
            return RedirectToPage("/Index");
        }

        SavedGamesList = _gameRepository.GetSavedGames(PlayerName);

        return Page();
    }
    
    public IActionResult OnPostLoadGame()
    {
        if (string.IsNullOrWhiteSpace(GameName))
        {
            return RedirectToPage("/SavedGames");
        }

        HttpContext.Session.SetString("CurrentGameName", GameName);
        return RedirectToPage("/Game", new { CurrentGameName = GameName });
    }
}