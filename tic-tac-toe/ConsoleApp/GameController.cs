using ConsoleUI;
using DAL;
using Domain;
using GameBrain;
using MenuSystem;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp;

public static class GameController
{
    private static readonly IConfigRepository ConfigRepo = new ConfigRepositoryHardcoded();
    // private static readonly GameRepositoryJson GameRepo = new GameRepositoryJson(); // For json
    private static readonly GameRepositoryDb GameRepo = new GameRepositoryDb(); // For database

    public static string SelectedGameType { get; private set; } = "Default";
    public static string PlayerName { get; set; } = "Default";
    public static GameConfiguration SelectedGameConfiguration { get; private set; } = default!;
    public static bool IsVersusAi { get; private set; } = false;
    public static bool AiVersusAi { get; private set; } = false;
    public static char LastPlayerPiece { get; private set; } = 'X';

    public static void SetSelectedGameType(string gameType)
    {
        SelectedGameType = gameType;
        IsVersusAi = gameType == "Player vs AI"; 
        AiVersusAi = gameType == "AI vs AI";
    }

    public static void SetSelectedGameConfiguration(GameConfiguration config)
    {
        config.PlayerName = PlayerName;
        config.GameType = SelectedGameType;
        SelectedGameConfiguration = config;
    }
    
    public static string StartGame()
    {
        var game = new TicTacTwoBrain(SelectedGameConfiguration);

        if (AiVersusAi)
        {
            Console.WriteLine("Starting AI vs AI...");
            return RunGameLoop(game, 'X', 'O'); 
        }
        else if (IsVersusAi)
        {
            Console.WriteLine("Starting Player vs AI...");
            return RunGameLoop(game, 'X', 'O');  
        }
        else
        {
            Console.WriteLine("Starting Player vs Player...");
            return RunGameLoop(game, 'X', 'O');  
        }
    }
    
    public static string LoadGame(string gameName)
    {
        var loadGame = GameRepo.LoadGame(gameName);
        var gameInstance = TicTacTwoBrain.FromJson(loadGame);
        SelectedGameConfiguration = gameInstance.Configuration;
        return RunGameLoop(gameInstance, 'X', 'O');
    }
    
    private static string RunGameLoop(TicTacTwoBrain gameInstance, char playerX, char playerO)
    {
        while (true)
        {
            Visualizer.DrawBoard(gameInstance);
            
            if (gameInstance.CheckWin(LastPlayerPiece))
            {
                Console.WriteLine($"Player {(gameInstance.IsXTurn ? "O" : "X")} wins!");
                Environment.Exit(0); 
            }

            if (gameInstance.CheckDraw())
            {
                Console.WriteLine("The game is a draw!");
                Environment.Exit(0); 
            }

            if (gameInstance.IsXTurn)
            {
                LastPlayerPiece = 'X';
                
                if (AiVersusAi)
                {
                    Console.WriteLine("AI X's turn.");
                    gameInstance.MakeAiMove();  
                    gameInstance.IsXTurn = false;
                }
                else
                {
                    Console.WriteLine("Player X's turn.");
                    var result = HandlePlayerTurn(gameInstance);
                    if (result == "RETURN") return "Return"; 
                    if (result == "CONTINUE") gameInstance.IsXTurn = false;
                    if (result == "MENU") return Menus.MainMenu.Run();
                }
            }
            else
            {
                LastPlayerPiece = 'O';
                
                if (AiVersusAi || IsVersusAi) 
                {
                    Console.WriteLine("AI O's turn.");
                    gameInstance.MakeAiMove();  
                    gameInstance.IsXTurn = true;
                }
                else 
                {
                    Console.WriteLine("Player O's turn.");
                    var result = HandlePlayerTurn(gameInstance);
                    if (result == "RETURN") return "Return"; 
                    if (result == "CONTINUE") gameInstance.IsXTurn = true;
                    if (result == "MENU") return Menus.MainMenu.Run();
                }
            }
        }
    }

    private static string HandlePlayerTurn(TicTacTwoBrain gameInstance)
    {
        switch (gameInstance.MovePieceAfterNMoves)
        {
            case >= 2:
                Console.WriteLine("Choose action: (1) Place piece, (S) Save game, (R) Return, (E) Exit game");
                break;
            case 1:
                Console.WriteLine("Choose action: (1) Place piece, (S) Save game, (E) Exit game");
                break;
            default:
                Console.WriteLine("Choose action: (1) Place piece, (2) Move grid, (3) Move piece, (S) Save game, (E) Exit game");
                break;
        }

        string? action = Console.ReadLine();

        switch (action?.ToUpper())
        {
            case "1":
                return HandlePlacePiece(gameInstance) ? "CONTINUE" : "INVALID";
            case "2":
                return HandleMoveGrid(gameInstance) ? "CONTINUE" : "INVALID";
            case "3":
                return HandleMovePiece(gameInstance) ? "CONTINUE" : "INVALID";
            case "S":
                return HandleSaveGame(gameInstance);
            case "R":
                return "RETURN"; 
            case "E":
                Environment.Exit(0);
                return "EXIT";
            default:
                Console.WriteLine("Invalid choice. Try again.");
                return "INVALID";
        }
    }
    
    private static bool HandleMoveGrid(TicTacTwoBrain gameInstance)
    {
        Console.WriteLine("Enter grid move offset (e.g., -1,0 for up):");
        var input = Console.ReadLine()?.Split(",");
        if (input?.Length == 2 &&
            int.TryParse(input[0], out int rowOffset) &&
            int.TryParse(input[1], out int colOffset))
        {
            if (gameInstance.MoveGrid(rowOffset, colOffset))
            {
                return true;
            }
            else
            {
                Console.WriteLine("Invalid grid movement.");
                return false;
            }
        }
        else
        {
            Console.WriteLine("Invalid input.");
            return false;
        }
    }

    private static bool HandleMovePiece(TicTacTwoBrain gameInstance)
    {
        Console.WriteLine("Enter source row/col and destination row/col (e.g., 1,1,2,2):");
        var input = Console.ReadLine()?.Split(",");
        if (input?.Length == 4 &&
            int.TryParse(input[0], out int fromRow) &&
            int.TryParse(input[1], out int fromCol) &&
            int.TryParse(input[2], out int toRow) &&
            int.TryParse(input[3], out int toCol))
        {
            if (gameInstance.MovePiece(fromRow, fromCol, toRow, toCol))
            {
                return true;
            }
            else
            {
                Console.WriteLine("Invalid piece movement.");
                return false;
            }
        }
        else
        {
            Console.WriteLine("Invalid input.");
            return false;
        }
    }

    private static string HandleSaveGame(TicTacTwoBrain gameInstance)
    {
        var gameState = gameInstance.GetGameStateAsJson(); 
        var gameType = SelectedGameType;
        var config = SelectedGameConfiguration;
        var playerName = PlayerName;
        GameRepo.SaveGame(gameState, config.Name, gameType, playerName); 

        Console.WriteLine("Game saved successfully.");
        Console.WriteLine("Do you want play again? (Y = yes, N = no)");
        
        while (true)
        {
            var input = Console.ReadLine()?.ToUpper();
            if (input == "Y")
            {
                return "MENU"; 
            }
            else if (input == "N")
            {
                Environment.Exit(0); 
                return "EXIT"; 
            }
            else
            {
                Console.WriteLine("Please enter 'Y' (yes) or 'N' (no).");
            }
        }
    }

    private static bool HandlePlacePiece(TicTacTwoBrain gameInstance)
    {
        Console.WriteLine("Enter row and column to place piece (e.g., 1,1):");
        var input = Console.ReadLine()?.Split(",");
        
        if (input?.Length == 2 &&
            int.TryParse(input[0], out int row) &&
            int.TryParse(input[1], out int col))
        {
            row -= 1;
            col -= 1;

            if (gameInstance.MakeAMove(row, col))
            {
                return true;
            }
            else
            {
                Console.WriteLine("Invalid move, try again.");
                return false;
            }
        }
        else
        {
            Console.WriteLine("Invalid input.");
            return false;
        }
    }
}

