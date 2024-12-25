using GameBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DAL;

namespace WebApp.Pages;

public class Game : PageModel
{
    // Properties
    public GameViewModel ViewModel { get; set; } = new();
    public TicTacTwoBrain GameInstance { get; set; } = null!;
    private static readonly GameRepositoryDb GameRepo = new();
    
    [BindProperty(SupportsGet = true)]
    public string? CurrentGameName { get; set; }

    // Page Handlers
    public void OnGet()
    {
        CurrentGameName = HttpContext.Session.GetString("gameName") ?? CurrentGameName;

        if (string.IsNullOrEmpty(CurrentGameName))
        {
            InitializeGame();
        }
        else
        {
            LoadGameFromDatabase(CurrentGameName);
        }
    }

    public IActionResult OnPostSetPiece(int x, int y)
    {
        LoadGameFromDatabase(CurrentGameName!);
        ProcessPlayerMove(y, x);
        UpdateViewModel();
        return Page();
    }

    public IActionResult OnPostAiMove()
    {
        // if (!EnsureGameName()) return Content("Error: CurrentGameName is not set.");

        LoadGameFromDatabase(CurrentGameName!);
        GameInstance.MakeAiMove();
        HandleEndGameCondition('O');
        UpdateViewModel();
        return Page();
    }

    public IActionResult OnPostMovePiece(int fromRow, int fromCol, int toRow, int toCol)
    {
        // if (!EnsureGameName()) return Content("Error: CurrentGameName is not set.");
        
        LoadGameFromDatabase(CurrentGameName!);

        if (GameInstance.MovePiece(fromRow, fromCol, toRow, toCol))
        {
            HandleEndGameCondition(GameInstance.IsXTurn ? 'X' : 'O');
        }
        else
        {
            TempData["ErrorMessage"] = "Invalid piece movement.";
        }

        UpdateViewModel();
        return Page();
    }

    public IActionResult OnPostMoveGrid(string direction)
    {
        // if (!EnsureGameName()) return Content("Error: CurrentGameName is not set.");
        
        LoadGameFromDatabase(CurrentGameName!);
        MoveGrid(direction);
        HandleEndGameCondition(GameInstance.IsXTurn ? 'X' : 'O');
        UpdateViewModel();
        return Page();
    }

    public IActionResult OnPostSaveGame()
    {
        LoadGameFromDatabase(CurrentGameName!);
        SaveGameToDatabase();
        return Content($"Game saved successfully with name: {CurrentGameName}");
    }

    // Private Helpers
    private void InitializeGame()
    {
        GameInstance = new TicTacTwoBrain(NewGame.GameConfig);
        CurrentGameName = SaveGameToDatabase();
        UpdateViewModel();
    }

    private string SaveGameToDatabase()
    {
        var gameState = GameInstance.GetGameStateAsJson();
        var gameName = GameRepo.SaveGame(gameState, 
                                         GameInstance.Configuration.BoardType, 
                                         GameInstance.Configuration.GameType, 
                                         GameInstance.Configuration.PlayerName);
        return gameName;
    }

    private void LoadGameFromDatabase(string gameName)
    {
        var gameStateJson = GameRepo.LoadGame(gameName);
        GameInstance = TicTacTwoBrain.FromJson(gameStateJson);
        UpdateViewModel();
    }

    private void ProcessPlayerMove(int row, int col)
    {
        if (GameInstance.MakeAMove(row, col))
        {
            HandleEndGameCondition(GameInstance.IsXTurn ? 'X' : 'O');
        }
    }

    private void HandleEndGameCondition(char lastPlayer)
    {
        if (GameInstance.CheckWin(lastPlayer))
        {
            TempData["WinMessage"] = $"Player {lastPlayer} wins!";
            ResetGame();
        }
        else if (GameInstance.CheckDraw())
        {
            TempData["WinMessage"] = "It is a draw!";
            ResetGame();
        }
        else
        {
            GameInstance.IsXTurn = !GameInstance.IsXTurn;
            GameRepo.UpdateGame(CurrentGameName!, GameInstance.GetGameStateAsJson());
        }
    }

    private void MoveGrid(string direction)
    {
        var moves = new Dictionary<string, (int dx, int dy)>
        {
            {"up", (-1, 0)}, {"down", (1, 0)}, 
            {"left", (0, -1)}, {"right", (0, 1)}, 
            {"up_left", (-1, -1)}, {"up_right", (-1, 1)}, 
            {"down_left", (1, -1)}, {"down_right", (1, 1)}
        };

        if (moves.TryGetValue(direction, out var move))
        {
            GameInstance.MoveGrid(move.dx, move.dy);
        }
    }

    private void ResetGame()
    {
        GameRepo.DeleteGame(CurrentGameName!);
        HttpContext.Session.Remove("CurrentGameName");
        InitializeGame();
    }

    private void UpdateViewModel()
    {
        ViewModel = new GameViewModel
        {
            DimX = GameInstance.DimX,
            DimY = GameInstance.DimY,
            SmallBoardStartX = GameInstance.SmallBoardPosX,
            SmallBoardStartY = GameInstance.SmallBoardPosY,
            SmallBoardEndX = GameInstance.SmallBoardPosX + GameInstance.SmallBoardWidth,
            SmallBoardEndY = GameInstance.SmallBoardPosY + GameInstance.SmallBoardHeight,
            GameType = GameInstance.Configuration.GameType,
            PlayerName = GameInstance.Configuration.PlayerName,
            BoardType = GameInstance.Configuration.BoardType
        };
    }

    // private bool EnsureGameName()
    // {
    //     return !string.IsNullOrEmpty(CurrentGameName);
    // }
}

public class GameViewModel
{
    public int DimX { get; set; }
    public int DimY { get; set; }
    public int SmallBoardStartX { get; set; }
    public int SmallBoardStartY { get; set; }
    public int SmallBoardEndX { get; set; }
    public int SmallBoardEndY { get; set; }
    public string GameType { get; set; } = string.Empty;
    public string PlayerName { get; set; } = string.Empty;
    public string BoardType { get; set; } = string.Empty;
}
