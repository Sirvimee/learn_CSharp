namespace DAL
{
    public class GameRepositoryJson : IGameRepository
    {
        public string SaveGame(string jsonStateString, string gameConfigName, string gameType, string playerName)
        {
            var directoryPath = FileHelper.BasePath;
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var fileName = directoryPath +
                           playerName + "_" +
                           gameType + "_" + 
                           gameConfigName + "_" +
                           DateTime.Now.ToString("dd/MM/yyy_HH-mm-ss") +
                           FileHelper.GameExtension;

            File.WriteAllText(fileName, jsonStateString);
            
            return Path.GetFileNameWithoutExtension(fileName);
        }
        
        public void UpdateGame(string gameName, string jsonStateString, DateTime? deletedAt)
        {
            var fullPath = FileHelper.BasePath + gameName + FileHelper.GameExtension;
            File.WriteAllText(fullPath, jsonStateString);
        }
        
        public void DeleteGame(string gameName)
        {
            var fullPath = FileHelper.BasePath + gameName + FileHelper.GameExtension;
            File.Delete(fullPath);
        }

        public List<string?> GetSavedGames(string playerName)
        {
            var directoryPath = FileHelper.BasePath;
            if (!Directory.Exists(directoryPath) || Directory.GetFiles(directoryPath).Length == 0)
            {
                return new List<string?>();
            }
            
            return Directory
                .GetFiles(directoryPath, "*" + FileHelper.GameExtension)
                .Select(Path.GetFileNameWithoutExtension)
                .Where(fileName => fileName?.StartsWith(playerName + "_") ?? false) 
                .ToList();
        }

        public string LoadGame(string gameFileName)
        {
            var fullPath = FileHelper.BasePath + gameFileName + FileHelper.GameExtension;
            return File.ReadAllText(fullPath);
        }
    }
}