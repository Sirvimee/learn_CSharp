using Domain;

namespace DAL;

public class GameRepositoryDb : IGameRepository
{
    public string SaveGame(string jsonStateString, string gameConfigName, string gameType, string playerName)
    {
        using var dbContext = new AppDbContextFactory().CreateDbContext(new string[] { });
        
        var game = new Game
        {
            GameName = playerName + " " + gameType + " " + gameConfigName + " " + DateTime.Now.ToString("dd.MM.yyyy HH-mm-ss"),
            GameState = jsonStateString,
        };
        
        dbContext.Games.Add(game);
        dbContext.SaveChanges();
        
        return game.GameName;
    }
    
    public void UpdateGame(string gameName, string jsonStateString, DateTime? deletedAt)
    {
        using var dbContext = new AppDbContextFactory().CreateDbContext(new string[] { });

        var game = dbContext.Games.FirstOrDefault(g => g.GameName == gameName);
        if (game == null)
        {
            throw new Exception($"Game with name {gameName} not found in database.");
        }

        game.GameState = jsonStateString;
        game.DeletedAt = deletedAt;
        dbContext.SaveChanges();
    }
    
    public List<string> GetSavedGames(string playerName)
    {
        using var dbContext = new AppDbContextFactory().CreateDbContext(new string[] { });

        return dbContext.Games
            .Where(g => g.GameName.StartsWith(playerName + " ") && g.DeletedAt == null) 
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
    
    public void DeleteGame(string gameName)
    {
        using var dbContext = new AppDbContextFactory().CreateDbContext(new string[] { });

        var game = dbContext.Games.FirstOrDefault(g => g.GameName == gameName);
        if (game == null)
        {
            throw new Exception($"Game with name {gameName} not found in database.");
        }

        dbContext.Games.Remove(game);
        dbContext.SaveChanges();
    }
}