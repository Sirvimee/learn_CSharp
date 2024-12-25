namespace GameBrain;

public class GameConfiguration()
{
    public string Name { get; set; } = "Default";
    public int BoardSizeWidth { get; set; } = 3;
    public int BoardSizeHeight { get; set; } = 3;
    public int GridSizeWidth { get; set; } = 3;
    public int GridSizeHeight { get; set; } = 3;

    public int WinCondition { get; set; } = 3;

    public string BoardType { get; set; } = "Default"; 
    
    public string GameType { get; set; } = "Default";
    
    public string PlayerName { get; set; } = "Default";

    public override string ToString() =>
        $"Board {BoardSizeWidth}x{BoardSizeHeight}, " +
        $"to win: {WinCondition}, ";
}