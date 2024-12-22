namespace DAL
{
    public class GameRepositoryJson : IGameRepository
    {
        public void SaveGame(string jsonStateString, string gameConfigName, string gameType, string playerName)
        {
            var directoryPath = FileHelper.BasePath;
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var fileName = directoryPath +
                           playerName + " " +
                           gameType + " " + 
                           gameConfigName + " " +
                           DateTime.Now.ToString("dd/MM/yyy_HH-mm-ss") +
                           FileHelper.GameExtension;

            File.WriteAllText(fileName, jsonStateString);
        }

        public List<string> GetSavedGames()
        {
            var directoryPath = FileHelper.BasePath;
            if (!Directory.Exists(directoryPath) || Directory.GetFiles(directoryPath).Length == 0)
            {
                return new List<string>();
            }

            return Directory
                .GetFiles(directoryPath, "*" + FileHelper.GameExtension)
                .Select(Path.GetFileNameWithoutExtension)
                .ToList()!;
        }

        public string LoadGame(string gameFileName)
        {
            var fullPath = FileHelper.BasePath + gameFileName + FileHelper.GameExtension;
            return File.ReadAllText(fullPath);
        }
    }
}