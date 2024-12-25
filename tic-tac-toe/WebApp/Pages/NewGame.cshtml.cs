using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GameBrain;

namespace WebApp.Pages;

public class NewGame : PageModel
{
    [BindProperty(SupportsGet = true)] 
    public string PlayerName { get; set; } = "Default";
    
    [BindProperty]
    public string GameType { get; set; } = "Default";
    
    [BindProperty]
    public string BoardType { get; set; } = "Default";
    
    public string? Error { get; set; }
    
    public static GameConfiguration GameConfig { get; private set; } = new GameConfiguration();
    private static readonly ConfigRepositoryHardcoded ConfigRepo = new ConfigRepositoryHardcoded();
    
    public IActionResult OnPost()
    {
        if (string.IsNullOrEmpty(GameType) || string.IsNullOrEmpty(BoardType))
        {
            Error = "Please select both a game type and a board type.";
            return Page();
        }
        
        var config = ConfigRepo.GetConfigurationByName(BoardType);
        GameConfig = new GameConfiguration
        {
            PlayerName = PlayerName,
            GameType = GameType,
            BoardType = BoardType,
            BoardSizeWidth = config.BoardSizeWidth,
            BoardSizeHeight = config.BoardSizeHeight,
            GridSizeWidth = config.GridSizeWidth,
            GridSizeHeight = config.GridSizeHeight,
            WinCondition = config.WinCondition
        };
        
        return RedirectToPage("/Game");
    }
}