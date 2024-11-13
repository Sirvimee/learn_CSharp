using Domain;

namespace DAL;

public class GameRepositoryDb : IGameRepositoryDb
{
    public void SaveGame(string gameState, string configName)
    {
        using var dbContext = new AppDbContextFactory().CreateDbContext(new string[] { });
        
        var config = dbContext.Configs.FirstOrDefault(c => c.Name == configName);
        if (config == null)
        {
            throw new Exception($"Config with name {configName} not found.");
        }
        
        var game = new Game
        {
            GameName = configName + " " + DateTime.Now.ToString("dd.MM.yyyy HH-mm-ss"),
            GameState = gameState,
            ConfigId = config.Id
        };
        
        dbContext.Games.Add(game);
        dbContext.SaveChanges();
    }
    
    public List<string> GetSavedGames()
    {
        using var dbContext = new AppDbContextFactory().CreateDbContext(new string[] { });
        
        return dbContext.Games
            .Select(g => g.GameName)
            .Distinct() 
            .ToList();
    }
    
    public string LoadGame(string gameName)
    {
        using var dbContext = new AppDbContextFactory().CreateDbContext(new string[] { });

        var game = dbContext.Games.FirstOrDefault(g => g.GameName == gameName);
        if (game == null)
        {
            throw new Exception($"Game with name {gameName} not found in database.");
        }

        return game.GameState;
    }
}