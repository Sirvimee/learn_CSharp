namespace GameBrain;

public record struct GameConfiguration()
{
    public string Name { get; set; } = default!;
    
    public int BoardSizeWidth { get; set; } = 3;
    public int BoardSizeHeight { get; set; } = 3;
    public int GridSizeWidth { get; set; } = 3;
    public int GridSizeHeight { get; set; } = 3;

    public int WinCondition { get; set; } = 3;
    public int MovePieceAfterNMoves { get; set; } = 0;

    public string BoardType { get; set; } = "Default"; 

    public override string ToString() =>
        $"Board {BoardSizeWidth}x{BoardSizeHeight}, " +
        $"to win: {WinCondition}, " +
        $"can move piece after {MovePieceAfterNMoves} moves";
}