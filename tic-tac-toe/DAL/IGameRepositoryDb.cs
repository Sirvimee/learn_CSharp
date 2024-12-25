namespace DAL;

public interface IGameRepositoryDb
{
    public int SaveGame(string gameState, string configName);
}