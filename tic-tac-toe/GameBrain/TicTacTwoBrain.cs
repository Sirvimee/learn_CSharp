namespace GameBrain;

public class TicTacTwoBrain
{
    public int DimX { get; set; }
    public int DimY { get; set; }
    public char[,] GameBoard { get; set; }
    public bool IsXTurn { get; set; }

    public int SmallBoardPosX { get; set; }
    public int SmallBoardPosY { get; set; }
    public int SmallBoardWidth { get; set; }
    public int SmallBoardHeight { get; set; }

    public GameConfiguration Configuration { get; }

    public TicTacTwoBrain(GameConfiguration config)
    {
        Configuration = config;
        DimX = config.BoardSizeWidth;
        DimY = config.BoardSizeHeight;
        GameBoard = new char[DimX, DimY];
        IsXTurn = true;

        SmallBoardWidth = config.GridSizeWidth;
        SmallBoardHeight = config.GridSizeHeight;

        // Set SmallBoard position to the center of the BigBoard
        SmallBoardPosX = (DimX - SmallBoardWidth) / 2;
        SmallBoardPosY = (DimY - SmallBoardHeight) / 2;

        for (var i = 0; i < DimX; i++)
        {
            for (var j = 0; j < DimY; j++)
            {
                GameBoard[i, j] = '.';
            }
        }
    }

    public bool MakeAMove(int row, int col)
    {
        if (GameBoard[row, col] == '.')
        {
            GameBoard[row, col] = IsXTurn ? 'X' : 'O';
            return true;
        }
        return false;
    }

    public bool MoveGrid(int rowOffset, int colOffset)
    {
        int newPosX = SmallBoardPosX + colOffset;
        int newPosY = SmallBoardPosY + rowOffset;

        if (newPosX >= 0 && newPosX + SmallBoardWidth <= DimX &&
            newPosY >= 0 && newPosY + SmallBoardHeight <= DimY)
        {
            SmallBoardPosX = newPosX;
            SmallBoardPosY = newPosY;
            return true;
        }
        return false;
    }

    public bool CheckWin()
    {
        // Implement win condition check logic here
        return false;
    }

    public bool MovePiece(int fromRow, int fromCol, int toRow, int toCol)
    {
        throw new NotImplementedException();
    }
}