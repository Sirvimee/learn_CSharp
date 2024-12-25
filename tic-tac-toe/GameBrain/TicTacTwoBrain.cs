using System.Text.Json;

namespace GameBrain
{
    public class TicTacTwoBrain
    {
        public int DimX { get; set; }
        public int DimY { get; set; }
        public List<List<char>> GameBoard { get; set; }
        public bool IsXTurn { get; set; }

        public int SmallBoardPosX { get; set; }
        public int SmallBoardPosY { get; set; }
        public int SmallBoardWidth { get; set; }
        public int SmallBoardHeight { get; set; }
        
        public string GameType { get; set; } = null!;
        
        public int MovePieceAfterNMoves { get; set; } = 2;
        public GameConfiguration Configuration { get; set; } = null!;

        public TicTacTwoBrain()
        {
            GameBoard = new List<List<char>>();
        }

        public TicTacTwoBrain(GameConfiguration config)
        {
            Configuration = config;
            DimX = config.BoardSizeWidth;
            DimY = config.BoardSizeHeight;
            GameBoard = InitializeGameBoard(DimX, DimY);
            GameType = config.GameType;
            IsXTurn = true;

            SmallBoardWidth = config.GridSizeWidth;
            SmallBoardHeight = config.GridSizeHeight;

            // Set SmallBoard position to the center of the BigBoard
            SmallBoardPosX = (DimX - SmallBoardWidth) / 2;
            SmallBoardPosY = (DimY - SmallBoardHeight) / 2;
        }

        private List<List<char>> InitializeGameBoard(int dimX, int dimY)
        {
            var board = new List<List<char>>();
            for (var i = 0; i < dimX; i++)
            {
                var row = new List<char>();
                for (var j = 0; j < dimY; j++)
                {
                    row.Add('.');
                }
                board.Add(row);
            }
            return board;
        }

        public string GetGameStateAsJson()
        {
            return JsonSerializer.Serialize(this);
        }

        public static TicTacTwoBrain FromJson(string jsonState)
        {
            var result = JsonSerializer.Deserialize<TicTacTwoBrain>(jsonState);
            if (result == null)
            {
                throw new InvalidOperationException("Deserialization failed, resulting in a null object.");
            }
            return result;
        }
        
        public bool MakeAiMove()
        {
            var aiPlayer = IsXTurn ? 'X' : 'O';
            // Check if AI can win
            for (var row = 0; row < DimY; row++)
            {
                for (var col = 0; col < DimX; col++)
                {
                    if (GameBoard[row][col] == '.')
                    {
                        GameBoard[row][col] = aiPlayer;
                        
                        if (CheckWin(aiPlayer))
                        {
                            return true;
                        }
                        GameBoard[row][col] = '.'; 
                    }
                }
            }

            // Check if Player X (opponent) can win, then block
            char opponent = aiPlayer == 'X' ? 'O' : 'X'; 
            for (var row = 0; row < DimY; row++)
            {
                for (var col = 0; col < DimX; col++)
                {
                    if (GameBoard[row][col] == '.')
                    {
                        GameBoard[row][col] = opponent;
                        if (CheckWin(opponent))
                        {
                            GameBoard[row][col] = aiPlayer; 
                            if (!IsXTurn) MovePieceAfterNMoves--;
                            return true;
                        }
                        GameBoard[row][col] = '.'; 
                    }
                }
            }

            // If AI can't win or block, make a simple move
            for (var row = 0; row < DimY; row++)
            {
                for (var col = 0; col < DimX; col++)
                {
                    if (GameBoard[row][col] == '.')
                    {
                        GameBoard[row][col] = aiPlayer; 
                        Console.WriteLine($"AI ({aiPlayer}) moves to ({row + 1}, {col + 1})");
                        if (!IsXTurn) MovePieceAfterNMoves--;
                        return true;
                    }
                }
            }
            return false;
        }

        public bool MakeAMove(int row, int col)
        {
            if (row < 0 || row >= DimX || col < 0 || col >= DimY)
            {
                Console.WriteLine("Invalid move. Try again.");
                return false;
            }

            if (GameBoard[row][col] == '.')
            {
                GameBoard[row][col] = IsXTurn ? 'X' : 'O';
                if (!IsXTurn) MovePieceAfterNMoves--;
                return true;
            }
            else
            {
                Console.WriteLine("Invalid move. Try again.");
                return false;
            }
        }
        
        public bool MovePiece(int fromRow, int fromCol, int toRow, int toCol)
        {
            fromRow -= 1;
            fromCol -= 1;
            toRow -= 1;
            toCol -= 1;
            
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
            if (GameBoard[fromRow][fromCol] != currentPlayerPiece)
            {
                Console.WriteLine("You can only move your own piece. Try again.");
                return false;
            }

            if (GameBoard[toRow][toCol] != '.')
            {
                Console.WriteLine("Destination cell is not empty. Try again.");
                return false;
            }

            GameBoard[fromRow][fromCol] = '.';
            GameBoard[toRow][toCol] = currentPlayerPiece;

            return true;
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

        public bool CheckWin(char playerPiece)
        {
            int winCondition = Configuration.WinCondition;

            // Horizontal check
            for (int row = SmallBoardPosY; row < SmallBoardPosY + SmallBoardHeight; row++)
            {
                for (int col = SmallBoardPosX; col <= SmallBoardPosX + SmallBoardWidth - winCondition; col++)
                {
                    if (Enumerable.Range(0, winCondition).All(k => GameBoard[row][col + k] == playerPiece))
                    {
                        return true;
                    }
                }
            }

            // Vertical check
            for (int col = SmallBoardPosX; col < SmallBoardPosX + SmallBoardWidth; col++)
            {
                for (int row = SmallBoardPosY; row <= SmallBoardPosY + SmallBoardHeight - winCondition; row++)
                {
                    if (Enumerable.Range(0, winCondition).All(k => GameBoard[row + k][col] == playerPiece))
                    {
                        return true;
                    }
                }
            }

            // Diagonal (top-left to bottom-right)
            for (int row = SmallBoardPosY; row <= SmallBoardPosY + SmallBoardHeight - winCondition; row++)
            {
                for (int col = SmallBoardPosX; col <= SmallBoardPosX + SmallBoardWidth - winCondition; col++)
                {
                    if (Enumerable.Range(0, winCondition).All(k => GameBoard[row + k][col + k] == playerPiece))
                    {
                        return true;
                    }
                }
            }

            // Diagonal (bottom-left to top-right)
            for (int row = SmallBoardPosY + winCondition - 1; row < SmallBoardPosY + SmallBoardHeight; row++)
            {
                for (int col = SmallBoardPosX; col <= SmallBoardPosX + SmallBoardWidth - winCondition; col++)
                {
                    if (Enumerable.Range(0, winCondition).All(k => GameBoard[row - k][col + k] == playerPiece))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        
        public bool CheckDraw()
        {
            return GameBoard.All(row => row.All(cell => cell != '.'));
        }
        
        public char GetPieceAt(int x, int y)
        {
            return GameBoard[y][x];
        }
    }
}