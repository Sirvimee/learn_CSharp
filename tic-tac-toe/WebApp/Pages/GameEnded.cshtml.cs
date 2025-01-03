using DAL;
using GameBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class GameEnded : PageModel
{
    private readonly GameRepositoryDb _gameRepository; // For Database
    // private readonly GameRepositoryJson _gameRepository; // For Json
    public TicTacTwoBrain GameInstance { get; set; } = null!;
    
    [BindProperty(SupportsGet = true)] public string? GameName { get; set; }
    [BindProperty(SupportsGet = true)] public string? PlayerName { get; set; }
    
    public int DimX { get; set; }
    public int DimY { get; set; }
    public int SmallBoardStartX { get; set; }
    public int SmallBoardStartY { get; set; }
    public int SmallBoardEndX { get; set; }
    public int SmallBoardEndY { get; set; }
    public string GameType { get; set; } = "Default";
    public string BoardType { get; set; } = "Default";
    
    public GameEnded(GameRepositoryDb gameRepository)
    {
        _gameRepository = gameRepository;
    }
    
    // public GameEnded(GameRepositoryJson gameRepository)
    // {
    //     _gameRepository = gameRepository;
    // }
    public void OnGet()
    {
        LoadGameFromDatabase(GameName!);
    }
    
    private void LoadGameFromDatabase(string gameName)
    {
        var gameStateJson = _gameRepository.LoadGame(gameName);

        GameInstance = TicTacTwoBrain.FromJson(gameStateJson);
        GameType = GameInstance.Configuration.GameType;
        BoardType = GameInstance.Configuration.BoardType;

        DimX = GameInstance.DimX;
        DimY = GameInstance.DimY;

        SetBoardDimensions();
    }
    
    private void SetBoardDimensions()
    {
        SmallBoardStartX = GameInstance.SmallBoardPosX;
        SmallBoardStartY = GameInstance.SmallBoardPosY;
        SmallBoardEndX = SmallBoardStartX + GameInstance.SmallBoardWidth;
        SmallBoardEndY = SmallBoardStartY + GameInstance.SmallBoardHeight;
    }
}