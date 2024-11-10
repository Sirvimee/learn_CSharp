namespace DAL
{
    public class GameRepositoryJson : IGameRepository
    {
        public void SaveGame(string jsonStateString, string gameConfigName)
        {
            var directoryPath = FileHelper.BasePath;
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var fileName = directoryPath +
                           gameConfigName + " " +
                           DateTime.Now.ToString("yyyyMMdd_HHmmss") +
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