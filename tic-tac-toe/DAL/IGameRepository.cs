namespace DAL;

public interface IGameRepository
{
    public string SaveGame(string jsonStateString, string gameConfigName, string gameType, string playerName);
}