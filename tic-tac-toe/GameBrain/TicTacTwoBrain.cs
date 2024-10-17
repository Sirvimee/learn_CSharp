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

    public int XMoveCount { get; set; }
    public int OMoveCount { get; set; }

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
        if (row < 0 || row >= DimX || col < 0 || col >= DimY)
        {
            Console.WriteLine("Invalid move. Try again.");
            return false;
        }

        if (GameBoard[row, col] == '.')
        {
            GameBoard[row, col] = IsXTurn ? 'X' : 'O';
            if (IsXTurn) XMoveCount++;
            else OMoveCount++;
            return true;
        }
        else
        {
            Console.WriteLine("Invalid move. Try again.");
            return false;
        }
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
    
    //TODO: Repair this method, now it's not working properly
    public bool CheckWin()
    {
        int winCondition = Configuration.Name == "Classical" ? 3 : 4;
        char currentPlayerPiece = IsXTurn ? 'X' : 'O';
        
        for (int row = SmallBoardPosY; row < SmallBoardPosY + SmallBoardHeight; row++)
        {
            for (int col = SmallBoardPosX; col <= SmallBoardPosX + SmallBoardWidth - winCondition; col++)
            {
                bool win = true;
                for (int k = 0; k < winCondition; k++)
                {
                    if (GameBoard[row, col + k] != currentPlayerPiece)
                    {
                        win = false;
                        break;
                    }
                }
                if (win)
                {
                    Console.WriteLine("Horizontal win detected.");
                    return true;
                }
            }
        }
        
        for (int col = SmallBoardPosX; col < SmallBoardPosX + SmallBoardWidth; col++)
        {
            for (int row = SmallBoardPosY; row <= SmallBoardPosY + SmallBoardHeight - winCondition; row++)
            {
                bool win = true;
                for (int k = 0; k < winCondition; k++)
                {
                    if (GameBoard[row + k, col] != currentPlayerPiece)
                    {
                        win = false;
                        break;
                    }
                }
                if (win)
                {
                    Console.WriteLine("Vertical win detected.");
                    return true;
                }
            }
        }
        
        for (int row = SmallBoardPosY; row <= SmallBoardPosY + SmallBoardHeight - winCondition; row++)
        {
            for (int col = SmallBoardPosX; col <= SmallBoardPosX + SmallBoardWidth - winCondition; col++)
            {
                bool win = true;
                for (int k = 0; k < winCondition; k++)
                {
                    if (GameBoard[row + k, col + k] != currentPlayerPiece)
                    {
                        win = false;
                        break;
                    }
                }
                if (win)
                {
                    Console.WriteLine("Diagonal win (top-left to bottom-right) detected.");
                    return true;
                }
            }
        }
        
        for (int row = SmallBoardPosY + winCondition - 1; row < SmallBoardPosY + SmallBoardHeight; row++)
        {
            for (int col = SmallBoardPosX; col <= SmallBoardPosX + SmallBoardWidth - winCondition; col++)
            {
                bool win = true;
                for (int k = 0; k < winCondition; k++)
                {
                    if (GameBoard[row - k, col + k] != currentPlayerPiece)
                    {
                        win = false;
                        break;
                    }
                }
                if (win)
                {
                    Console.WriteLine("Diagonal win (bottom-left to top-right) detected.");
                    return true;
                }
            }
        }

        return false;
    }


    public bool MovePiece(int fromRow, int fromCol, int toRow, int toCol)
    {
        if (fromRow < 0 || fromRow >= DimX || fromCol < 0 || fromCol >= DimY)
        {
            Console.WriteLine("Invalid source coordinates. Try again.");
            return false;
        }
        
        if (toRow < 0 || toRow >= DimX || toCol < 0 || toCol >= DimY)
        {
            Console.WriteLine("Invalid destination coordinates. Try again.");
            return false;
        }
        
        char currentPlayerPiece = IsXTurn ? 'X' : 'O';
        if (GameBoard[fromRow, fromCol] != currentPlayerPiece)
        {
            Console.WriteLine("You can only move your own piece. Try again.");
            return false;
        }
        
        if (GameBoard[toRow, toCol] != '.')
        {
            Console.WriteLine("Destination cell is not empty. Try again.");
            return false;
        }
        
        GameBoard[fromRow, fromCol] = '.';
        GameBoard[toRow, toCol] = currentPlayerPiece;

        return true;
    }
}