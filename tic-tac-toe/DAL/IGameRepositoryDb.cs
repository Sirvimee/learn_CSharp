namespace DAL;

public interface IGameRepositoryDb
{
    public void SaveGame(string gameState, string configName);
}