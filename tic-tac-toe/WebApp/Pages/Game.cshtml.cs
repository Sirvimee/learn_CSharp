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
    public GameConfiguration GameConfig { get; set; }
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
                TempData["WinMessage"] = $"Player {(GameInstance.IsXTurn ? "X" : "O")} wins!";
                GameRepo.DeleteGame(CurrentGameName!);
                HttpContext.Session.Remove("CurrentGameName");
                InitializeGame();
            }
            else if (GameInstance.CheckDraw())
            {
                TempData["WinMessage"] = "It is a draw!";
                GameRepo.DeleteGame(CurrentGameName!);
                HttpContext.Session.Remove("CurrentGameName");
                InitializeGame();
            }
            else
            {
                GameInstance.IsXTurn = !GameInstance.IsXTurn;
                GameRepo.UpdateGame(CurrentGameName!, GameInstance.GetGameStateAsJson());
            }
        }

        DimX = GameInstance.DimX;
        DimY = GameInstance.DimY;
        SetBoardDimensions();

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

        if (GameInstance.CheckWin('O'))
        {
            TempData["WinMessage"] = "AI wins!";
            GameRepo.DeleteGame(CurrentGameName);
            HttpContext.Session.Remove("CurrentGameName");
            InitializeGame();
        }
        else if (GameInstance.CheckDraw())
        {
            TempData["WinMessage"] = "It is a draw!";
            GameRepo.DeleteGame(CurrentGameName);
            HttpContext.Session.Remove("CurrentGameName");
            InitializeGame();
        }
        else
        {
            GameInstance.IsXTurn = !GameInstance.IsXTurn;
            GameRepo.UpdateGame(CurrentGameName, GameInstance.GetGameStateAsJson());
        }

        DimX = GameInstance.DimX;
        DimY = GameInstance.DimY;
        SetBoardDimensions();

        return Page();
    }

    private void InitializeGame()
    {
        GameConfig = NewGame.GameConfig;
        GameInstance = new TicTacTwoBrain(GameConfig);

        DimX = GameConfig.BoardSizeWidth;
        DimY = GameConfig.BoardSizeHeight;
        
        SetBoardDimensions();
        CurrentGameName = SaveGameToDatabase();
    }

    private string SaveGameToDatabase()
    {
        var gameState = GameInstance.GetGameStateAsJson();
        BoardType = GameInstance.Configuration.BoardType;
        GameType = GameInstance.Configuration.GameType;
        PlayerName = GameInstance.Configuration.PlayerName;
        var gameName =  GameRepo.SaveGame(gameState, BoardType, GameType, PlayerName);

        return gameName;
    }

    private void LoadGameFromDatabase(string gameName)
    {
        var gameStateJson = GameRepo.LoadGame(gameName);

        GameInstance = TicTacTwoBrain.FromJson(gameStateJson);
        GameType = GameInstance.Configuration.GameType;
        PlayerName = GameInstance.Configuration.PlayerName;
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

    public IActionResult OnPostSaveGame()
    {
        LoadGameFromDatabase(CurrentGameName!);
        var savedGameName = SaveGameToDatabase();
        GameRepo.DeleteGame(CurrentGameName!);
        CurrentGameName = savedGameName;
        // return Content($"Game saved successfully with name: {CurrentGameName}");
        
        TempData["GameSaved"] = $"Game saved successfully with name: {CurrentGameName}";
        HttpContext.Session.Remove("CurrentGameName");
        InitializeGame();
        return Page();
    }

    public IActionResult OnPostMoveGrid(string direction)
    {
        bool moved = direction switch
        {
            "up" => GameInstance.MoveGrid(-1, 0),
            "up_left" => GameInstance.MoveGrid(-1, -1),
            "up_right" => GameInstance.MoveGrid(-1, 1),
            "down" => GameInstance.MoveGrid(1, 0),
            "down_left" => GameInstance.MoveGrid(1, -1),
            "down_right" => GameInstance.MoveGrid(1, 1),
            "left" => GameInstance.MoveGrid(0, -1),
            "right" => GameInstance.MoveGrid(0, 1),
            _ => false
        };

        return Page();
    }

    public string GetButtonClickHandler(int x, int y) => $"SetPiece({x}, {y})";
}