using GameBrain;

namespace ConsoleUI;

public static class Visualizer
{
    public static void DrawBoard(TicTacTwoBrain gameInstance)
    {
        var config = gameInstance.Configuration;

        for (var y = 0; y < gameInstance.DimY; y++)
        {
            for (var x = 0; x < gameInstance.DimX; x++)
            {
                if ((config.BoardType == "Classical" && x >= 1 && x <= 2 && y >= 1 && y <= 3) ||
                    (config.BoardType == "Big board" && x >= 3 && x <= 5 && y >= 3 && y <= 6))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else
                {
                    Console.ResetColor();
                }

                Console.Write(" " + DrawGamePiece(gameInstance.GameBoard[x, y]) + " ");
                if (x == gameInstance.DimX - 1) continue;
                Console.Write("|");
            }

            Console.WriteLine();
            if (y == gameInstance.DimY - 1) continue;
            for (var x = 0; x < gameInstance.DimX; x++)
            {
                if ((config.BoardType == "Classical" && x >= 1 && x <= 3 && y >= 1 && y <= 2) ||
                    (config.BoardType == "Big board" && x >= 3 && x <= 5 && y >= 2 && y <= 6))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else
                {
                    Console.ResetColor();
                }

                Console.Write("---");
                if (x != gameInstance.DimX - 1)
                {
                    Console.Write("+");
                }
            }

            Console.WriteLine();
        }

        Console.ResetColor();
    }

    private static string DrawGamePiece(EGamePiece piece) =>
        piece switch
        {
            EGamePiece.O => "O",
            EGamePiece.X => "X",
            _ => " "
        };
}