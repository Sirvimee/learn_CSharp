using GameBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DAL;

namespace WebApp.Pages;

public class Game : PageModel
{
    public int DimX { get; set; }
    public int DimY { get; set; }
    public int SmallBoardStartX { get; set; }
    public int SmallBoardStartY { get; set; }
    public int SmallBoardEndX { get; set; }
    public int SmallBoardEndY { get; set; }

    public TicTacTwoBrain GameInstance { get; set; } = null!;
    public GameConfiguration GameConfig { get; set; } = new GameConfiguration();
    private static readonly GameRepositoryDb GameRepo = new GameRepositoryDb();

    public string GameType { get; set; } = "Default";
    public string PlayerName { get; set; } = "Default";
    public string BoardType { get; set; } = "Default";

    [BindProperty(SupportsGet = true)]
    public string? CurrentGameName { get; set; }
    
    public void OnGet()
    {
        CurrentGameName = HttpContext.Session.GetString("gameName") ?? CurrentGameName;

        if (string.IsNullOrEmpty(CurrentGameName))
        {
            InitializeGame();
            LoadGameFromDatabase(CurrentGameName!);
        }
        else
        {
            LoadGameFromDatabase(CurrentGameName);
        }
    }
    
    public IActionResult OnPostSetPiece(int x, int y)
    {
        LoadGameFromDatabase(CurrentGameName!);

        if (GameInstance.MakeAMove(y, x))
        {
            if (GameInstance.CheckWin(GameInstance.IsXTurn ? 'X' : 'O'))
            {
                GameRepo.UpdateGame(CurrentGameName!, GameInstance.GetGameStateAsJson(), DateTime.Now);
                return RedirectToPage("./GameEnded", new { 
                    playerName = (GameInstance.IsXTurn ? "X" : "O"), 
                    gameName = CurrentGameName });
            }
            else if (GameInstance.CheckDraw())
            {
                GameRepo.UpdateGame(CurrentGameName!, GameInstance.GetGameStateAsJson(), DateTime.Now);
                return RedirectToPage("./GameEnded", new { gameName = CurrentGameName });
            }
            else
            {
                GameInstance.IsXTurn = !GameInstance.IsXTurn;
                GameRepo.UpdateGame(CurrentGameName!, GameInstance.GetGameStateAsJson(), null);
            }
        }

        UpdateBoardDimensions();
        return Page();
    }
    
    public IActionResult OnPostAiMove()
    {
        if (string.IsNullOrEmpty(CurrentGameName))
        {
            return Content("Error: CurrentGameName is not set.");
        }

        LoadGameFromDatabase(CurrentGameName);
        GameInstance.MakeAiMove();

        if (GameInstance.CheckWin(GameInstance.IsXTurn ? 'X' : 'O'))
        {
            GameRepo.UpdateGame(CurrentGameName!, GameInstance.GetGameStateAsJson(), DateTime.Now);
            return RedirectToPage("./GameEnded", new {
                playerName = (GameInstance.IsXTurn ? "X" : "O"), 
                gameName = CurrentGameName
            });
        }
        else if (GameInstance.CheckDraw())
        {
            GameRepo.UpdateGame(CurrentGameName!, GameInstance.GetGameStateAsJson(), DateTime.Now);
            return RedirectToPage("./GameEnded", new { gameName = CurrentGameName });
        }
        else
        {
            GameInstance.IsXTurn = !GameInstance.IsXTurn;
            GameRepo.UpdateGame(CurrentGameName!, GameInstance.GetGameStateAsJson(), null);
        }

        UpdateBoardDimensions();
        return Page();
    }
    
    public IActionResult OnPostMovePiece(int fromRow, int fromCol, int toRow, int toCol)
    {
        if (string.IsNullOrEmpty(CurrentGameName))
        {
            return Content("Error: CurrentGameName is not set.");
        }

        LoadGameFromDatabase(CurrentGameName!);

        if (GameInstance.MovePiece(fromRow, fromCol, toRow, toCol))
        {
            if (GameInstance.CheckWin(GameInstance.IsXTurn ? 'X' : 'O'))
            {
                GameRepo.UpdateGame(CurrentGameName!, GameInstance.GetGameStateAsJson(), DateTime.Now);
                return RedirectToPage("./GameEnded", new {
                    playerName = (GameInstance.IsXTurn ? "X" : "O"), 
                    gameName = CurrentGameName
                });
            }
            else
            {
                GameInstance.IsXTurn = !GameInstance.IsXTurn;
                GameRepo.UpdateGame(CurrentGameName!, GameInstance.GetGameStateAsJson(), null);
            }
        }
        else
        {
            TempData["ErrorMessage"] = "Invalid piece movement.";
        }

        UpdateBoardDimensions();
        return Page();
    }
    
    private void InitializeGame()
    {
        GameConfig = NewGame.GameConfig;
        GameInstance = new TicTacTwoBrain(GameConfig);

        UpdateBoardDimensions();
        CurrentGameName = SaveGameToDatabase();
    }
    
    private string SaveGameToDatabase()
    {
        var gameState = GameInstance.GetGameStateAsJson();
        BoardType = GameInstance.Configuration.BoardType;
        GameType = GameInstance.Configuration.GameType;
        PlayerName = GameInstance.Configuration.PlayerName;
        var gameName = GameRepo.SaveGame(gameState, BoardType, GameType, PlayerName);

        return gameName;
    }
    
    private void LoadGameFromDatabase(string gameName)
    {
        var gameStateJson = GameRepo.LoadGame(gameName);

        GameInstance = TicTacTwoBrain.FromJson(gameStateJson);
        GameType = GameInstance.Configuration.GameType;
        PlayerName = GameInstance.Configuration.PlayerName;
        BoardType = GameInstance.Configuration.BoardType;

        UpdateBoardDimensions();
    }
    
    private void UpdateBoardDimensions()
    {
        DimX = GameInstance.DimX;
        DimY = GameInstance.DimY;
        SmallBoardStartX = GameInstance.SmallBoardPosX;
        SmallBoardStartY = GameInstance.SmallBoardPosY;
        SmallBoardEndX = SmallBoardStartX + GameInstance.SmallBoardWidth;
        SmallBoardEndY = SmallBoardStartY + GameInstance.SmallBoardHeight;
    }
    
    public IActionResult OnPostSaveGame()
    {
        LoadGameFromDatabase(CurrentGameName!);
        var savedGameName = SaveGameToDatabase();
        GameRepo.DeleteGame(CurrentGameName!);
        CurrentGameName = savedGameName;
        return Content($"Game saved successfully with name: {CurrentGameName}");
    }
    
    public IActionResult OnPostMoveGrid(string direction)
    {
        if (string.IsNullOrEmpty(CurrentGameName))
        {
            return Content("Error: CurrentGameName is not set.");
        }

        LoadGameFromDatabase(CurrentGameName);

        switch (direction)
        {
            case "up":
                GameInstance.MoveGrid(-1, 0);
                break;
            case "up_left":
                GameInstance.MoveGrid(-1, -1);
                break;
            case "up_right":
                GameInstance.MoveGrid(-1, 1);
                break;
            case "down_left":
                GameInstance.MoveGrid(1, -1);
                break;
            case "down_right":
                GameInstance.MoveGrid(1, 1);
                break;
            case "down":
                GameInstance.MoveGrid(1, 0);
                break;
            case "left":
                GameInstance.MoveGrid(0, -1);
                break;
            case "right":
                GameInstance.MoveGrid(0, 1);
                break;
        }

        if (GameInstance.CheckWin(GameInstance.IsXTurn ? 'X' : 'O'))
        {
            GameRepo.UpdateGame(CurrentGameName!, GameInstance.GetGameStateAsJson(), DateTime.Now);
            return RedirectToPage("./GameEnded", new
            {
                playerName = (GameInstance.IsXTurn ? "X" : "O"), 
                gameName = CurrentGameName
            });
        }
        else
        {
            GameInstance.IsXTurn = !GameInstance.IsXTurn;
            GameRepo.UpdateGame(CurrentGameName!, GameInstance.GetGameStateAsJson(), null);
        }

        UpdateBoardDimensions();
        return Page();
    }
    
    public string GetButtonClickHandler(int x, int y) => $"SetPiece({x}, {y})";
}