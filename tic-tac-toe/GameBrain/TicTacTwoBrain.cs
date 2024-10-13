namespace GameBrain;

public class TicTacTwoBrain
{
    private EGamePiece[,] _gameBoard;
    private EGamePiece _nextMoveBy { get; set; } = EGamePiece.X;
    private int _centerX;
    private int _centerY;

    public GameConfiguration Configuration { get; private set; }

    public TicTacTwoBrain(GameConfiguration gameConfiguration)
    {
        Configuration = gameConfiguration;
        _gameBoard = new EGamePiece[Configuration.BoardSizeWidth, Configuration.BoardSizeHeight];
        _centerX = Configuration.BoardSizeWidth / 2;
        _centerY = Configuration.BoardSizeHeight / 2;
    }

    public EGamePiece[,] GameBoard => GetBoard();
    public int DimX => _gameBoard.GetLength(0);
    public int DimY => _gameBoard.GetLength(1);
    public bool IsXTurn { get; set; } = true;

    private EGamePiece[,] GetBoard()
    {
        var copyOfBoard = new EGamePiece[_gameBoard.GetLength(0), _gameBoard.GetLength(1)];
        for (var x = 0; x < _gameBoard.GetLength(0); x++)
        {
            for (var y = 0; y < _gameBoard.GetLength(1); y++)
            {
                copyOfBoard[x, y] = _gameBoard[x, y];
            }
        }
        return copyOfBoard;
    }

    public bool MakeAMove(int x, int y)
    {
        if (_gameBoard[x, y] != EGamePiece.Empty)
        {
            return false;
        }

        _gameBoard[x, y] = _nextMoveBy;
        _nextMoveBy = _nextMoveBy == EGamePiece.X ? EGamePiece.O : EGamePiece.X;
        return true;
    }

    public bool MovePiece(int fromX, int fromY, int toX, int toY)
    {
        if (_gameBoard[fromX, fromY] != _nextMoveBy || _gameBoard[toX, toY] != EGamePiece.Empty)
        {
            return false;
        }

        _gameBoard[toX, toY] = _gameBoard[fromX, fromY];
        _gameBoard[fromX, fromY] = EGamePiece.Empty;
        _nextMoveBy = _nextMoveBy == EGamePiece.X ? EGamePiece.O : EGamePiece.X;
        return true;
    }

    public bool MoveGrid(int deltaX, int deltaY)
    {
        int newCenterX = _centerX + deltaX;
        int newCenterY = _centerY + deltaY;

        if (newCenterX < 0 || newCenterX >= DimX || newCenterY < 0 || newCenterY >= DimY)
        {
            return false;
        }

        _centerX = newCenterX;
        _centerY = newCenterY;
        _nextMoveBy = _nextMoveBy == EGamePiece.X ? EGamePiece.O : EGamePiece.X;
        return true;
    }

    public bool CheckWin()
    {
        // Implement win condition check logic here
        return false;
    }

    public void ResetGame()
    {
        _gameBoard = new EGamePiece[_gameBoard.GetLength(0), _gameBoard.GetLength(1)];
        _nextMoveBy = EGamePiece.X;
        _centerX = Configuration.BoardSizeWidth / 2;
        _centerY = Configuration.BoardSizeHeight / 2;
    }
    
}