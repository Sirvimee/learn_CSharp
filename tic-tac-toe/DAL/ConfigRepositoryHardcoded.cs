using GameBrain;

namespace DAL;

public class ConfigRepositoryHardcoded : IConfigRepository
{
    private List<GameConfiguration> _gameConfigurations = new List<GameConfiguration>()
    {
        new GameConfiguration()
        {
            Name = "Classical",
            BoardSizeWidth = 5,
            BoardSizeHeight = 5,
            GridSizeHeight = 3,
            GridSizeWidth = 3,
            WinCondition = 3,
            MovePieceAfterNMoves = 2,
            BoardType = "Classical"
        },
        new GameConfiguration()
        {
            Name = "Big board",
            BoardSizeWidth = 10,
            BoardSizeHeight = 10,
            GridSizeHeight = 4,
            GridSizeWidth = 4,
            WinCondition = 4,
            MovePieceAfterNMoves = 2,
            BoardType = "Big board"
        },
    };

    public List<string> GetConfigurationNames()
    {
        return _gameConfigurations
            .OrderBy(x => x.Name)
            .Select(config => config.Name)
            .ToList();
    }

    public GameConfiguration GetConfigurationByName(string name)
    {
        return _gameConfigurations.Single(c => c.Name == name);
    }
    
    public void SaveConfiguration(GameConfiguration gameConfig)
    {
    }
    
}
