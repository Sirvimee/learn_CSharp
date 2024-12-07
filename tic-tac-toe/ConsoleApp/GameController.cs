using ConsoleUI;
using DAL;
using GameBrain;
using MenuSystem;

namespace ConsoleApp;

public static class GameController
{
    private static readonly IConfigRepository ConfigRepo = new ConfigRepositoryHardcoded();
    private static readonly GameRepositoryJson GameRepo = new GameRepositoryJson(); // For json
    // private static readonly GameRepositoryDb GameRepo = new GameRepositoryDb(); // For database

    public static string SelectedGameType { get; private set; } = null!;
    public static GameConfiguration SelectedGameConfiguration { get; private set; } = default!;
    public static bool IsVersusAi { get; private set; } = false;
    public static bool AiVersusAi { get; private set; } = false;

    public static void SetSelectedGameType(string gameType)
    {
        SelectedGameType = gameType;
        IsVersusAi = gameType == "Player vs AI"; 
        AiVersusAi = gameType == "AI vs AI";
    }

    public static void SetSelectedGameConfiguration(GameConfiguration config)
    {
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

        if (gameInstance.IsXTurn)
        {
            if (AiVersusAi) // AI X turn
            {
                Console.WriteLine("AI X's turn.");
                MakeAiMove(gameInstance, playerX); 
                if (gameInstance.CheckWin(playerX))
                {
                    Visualizer.DrawBoard(gameInstance);
                    Console.WriteLine("AI X wins!");
                    return "Game over";
                }
                gameInstance.IsXTurn = false;
            }
            else if (IsVersusAi) // Player X vs AI O
            {
                Console.WriteLine("Player X's turn.");
                var result = HandlePlayerTurn(gameInstance);
                if (result == "RETURN") return "Return"; 
                if (result == "CONTINUE") gameInstance.IsXTurn = false;
                if (result == "MENU") return Menus.MainMenu.Run();
            }
            else // Player X vs Player O
            {
                Console.WriteLine("Player X's turn.");
                var result = HandlePlayerTurn(gameInstance);
                if (result == "RETURN") return "Return"; 
                if (result == "CONTINUE") gameInstance.IsXTurn = false;
                if (result == "MENU") return Menus.MainMenu.Run();
            }
        }
        else // Player O or AI O's turn
        {
            if (AiVersusAi) // AI O turn
            {
                Console.WriteLine("AI O's turn.");
                MakeAiMove(gameInstance, playerO);  
                if (gameInstance.CheckWin(playerO))
                {
                    Visualizer.DrawBoard(gameInstance);
                    Console.WriteLine("AI O wins!");
                    return "Game over";
                }
                gameInstance.IsXTurn = true;
            }
            else if (IsVersusAi) // Player O vs AI X
            {
                Console.WriteLine("AI O's turn.");
                MakeAiMove(gameInstance, playerO);  
                if (gameInstance.CheckWin(playerO))
                {
                    Visualizer.DrawBoard(gameInstance);
                    Console.WriteLine("AI O wins!");
                    return "Game over";
                }
                gameInstance.IsXTurn = true;
            }
            else // Player O vs Player X
            {
                Console.WriteLine("Player O's turn.");
                var result = HandlePlayerTurn(gameInstance);
                if (result == "RETURN") return "Return"; 
                if (result == "CONTINUE") gameInstance.IsXTurn = true;
                if (result == "MENU") return Menus.MainMenu.Run();
            }
        }

        // Check for draw (all cells filled, no winner)
        if (gameInstance.XMoveCount + gameInstance.OMoveCount == gameInstance.DimX * gameInstance.DimY)
        {
            Visualizer.DrawBoard(gameInstance);
            Console.WriteLine("The game is a draw!");
            break;
        }
    }

    return "Game over";
}

    private static string HandlePlayerTurn(TicTacTwoBrain gameInstance)
    {
        if ((gameInstance.IsXTurn && gameInstance.XMoveCount >= 2) ||
            (!gameInstance.IsXTurn && gameInstance.OMoveCount >= 2))
        {
            Console.WriteLine("Choose action: (1) Place piece, (2) Move grid, (3) Move piece, (S) Save game, (E) Exit game");
        }
        else if (gameInstance.XMoveCount < 1)
        {
            Console.WriteLine("Choose action: (1) Place piece, (S) Save game, (R) Return, (E) Exit game");
        }
        else
        {
            Console.WriteLine("Choose action: (1) Place piece, (S) Save game, (E) Exit game");
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
            fromRow -= 1;
            fromCol -= 1;
            toRow -= 1;
            toCol -= 1;
            
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
        GameRepo.SaveGame(gameState, gameType, SelectedGameConfiguration.Name); 

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

    private static void MakeAiMove(TicTacTwoBrain gameInstance, char aiPlayer)
{
    // Check if AI can win
    for (var row = 0; row < gameInstance.DimY; row++)
    {
        for (var col = 0; col < gameInstance.DimX; col++)
        {
            if (gameInstance.GameBoard[row][col] == '.')
            {
                gameInstance.GameBoard[row][col] = aiPlayer; 
                if (gameInstance.CheckWin(aiPlayer))
                {
                    Console.WriteLine($"AI ({aiPlayer}) wins by moving to ({row + 1}, {col + 1})");
                    return;
                }
                gameInstance.GameBoard[row][col] = '.'; 
            }
        }
    }

    // Check if Player X (opponent) can win, then block
    char opponent = aiPlayer == 'X' ? 'O' : 'X'; 
    for (var row = 0; row < gameInstance.DimY; row++)
    {
        for (var col = 0; col < gameInstance.DimX; col++)
        {
            if (gameInstance.GameBoard[row][col] == '.')
            {
                gameInstance.GameBoard[row][col] = opponent; 
                if (gameInstance.CheckWin(opponent))
                {
                    gameInstance.GameBoard[row][col] = aiPlayer; 
                    Console.WriteLine($"AI ({aiPlayer}) blocks {opponent} at ({row + 1}, {col + 1})");
                    return;
                }
                gameInstance.GameBoard[row][col] = '.'; 
            }
        }
    }

    // If AI can't win or block, make a simple move
    for (var row = 0; row < gameInstance.DimY; row++)
    {
        for (var col = 0; col < gameInstance.DimX; col++)
        {
            if (gameInstance.GameBoard[row][col] == '.')
            {
                gameInstance.GameBoard[row][col] = aiPlayer; 
                Console.WriteLine($"AI ({aiPlayer}) moves to ({row + 1}, {col + 1})");
                return;
            }
        }
    }
}

    private static bool HandlePlacePiece(TicTacTwoBrain gameInstance)
    {
        Console.WriteLine("Enter row and column to place piece (e.g., 1,1):");
        var input = Console.ReadLine()?.Split(",");
        char currentPlayerPiece = gameInstance.IsXTurn ? 'X' : 'O';
        if (input?.Length == 2 &&
            int.TryParse(input[0], out int row) &&
            int.TryParse(input[1], out int col))
        {
            row -= 1;
            col -= 1;

            if (gameInstance.MakeAMove(row, col))
            {
                if (gameInstance.CheckWin(currentPlayerPiece))
                {
                    Visualizer.DrawBoard(gameInstance);
                    Console.WriteLine($"Player {(gameInstance.IsXTurn ? "X" : "O")} wins!");
                    Environment.Exit(0); 
                }

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

