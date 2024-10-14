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
                if (x >= gameInstance.SmallBoardPosX && x < gameInstance.SmallBoardPosX + gameInstance.SmallBoardWidth &&
                    y >= gameInstance.SmallBoardPosY && y < gameInstance.SmallBoardPosY + gameInstance.SmallBoardHeight)
                {
                    Console.BackgroundColor = ConsoleColor.Cyan;
                    Console.ForegroundColor = ConsoleColor.Black;
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

    private static string DrawGamePiece(char piece) =>
        piece switch
        {
            'O' => "O",
            'X' => "X",
            _ => " "
        };
}