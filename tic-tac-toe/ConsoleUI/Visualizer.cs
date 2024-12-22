using GameBrain;

namespace ConsoleUI;

public static class Visualizer
{
    public static void DrawBoard(TicTacTwoBrain gameInstance)
    {
        int smallBoardStartX = Math.Max(gameInstance.SmallBoardPosX, 0);
        int smallBoardStartY = Math.Max(gameInstance.SmallBoardPosY, 0);
        int smallBoardEndX = Math.Min(gameInstance.SmallBoardPosX + gameInstance.SmallBoardWidth, gameInstance.DimX);
        int smallBoardEndY = Math.Min(gameInstance.SmallBoardPosY + gameInstance.SmallBoardHeight, gameInstance.DimY);

        for (var y = 0; y < gameInstance.DimY; y++)
        {
            for (var x = 0; x < gameInstance.DimX; x++)
            {
                if (x >= smallBoardStartX && x < smallBoardEndX &&
                    y >= smallBoardStartY && y < smallBoardEndY)
                {
                    Console.BackgroundColor = ConsoleColor.Cyan;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.ResetColor();
                }
                
                Console.Write(" " + DrawGamePiece(gameInstance.GameBoard[y][x]) + " ");
                
                Console.ResetColor();
                if (x < gameInstance.DimX - 1)
                {
                    Console.Write("|");
                }
            }

            Console.WriteLine();
            
            if (y < gameInstance.DimY - 1)
            {
                Console.ResetColor();
                for (var x = 0; x < gameInstance.DimX; x++)
                {
                    Console.Write("---");
                    if (x < gameInstance.DimX - 1)
                    {
                        Console.Write("+");
                    }
                }
                Console.WriteLine();
            }
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