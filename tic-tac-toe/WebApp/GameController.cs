namespace WebApp
{
    public static class GameController
    {
        // Valitud mängu tüüp (nt "Player vs Player", "Player vs AI")
        public static string? SelectedGameType { get; private set; }
        
        // Valitud mängu konfiguratsioon (nt "Big board", "Classical")
        public static string? SelectedGameConfiguration { get; private set; }
        
        // Mängija nimi
        public static string PlayerName { get; private set; } = "DefaultPlayer";

        // Salvestatud mängude hoidla
        private static readonly List<string> SavedGames = new List<string>
        {
            "Game1",
            "Game2",
            "Game3"
        };

        // Meetod mängu tüübi määramiseks
        public static void SetSelectedGameType(string? gameType)
        {
            SelectedGameType = gameType;
        }

        // Meetod mängu konfiguratsiooni määramiseks
        public static void SetSelectedGameConfiguration(string config)
        {
            SelectedGameConfiguration = config;
        }

        // Mängu alustamine valitud konfiguratsioonidega
        public static async Task<string> StartGame()
        {
            if (string.IsNullOrEmpty(SelectedGameType) || string.IsNullOrEmpty(SelectedGameConfiguration))
            {
                return "Please select a game type and configuration before starting the game.";
            }

            // Loogika mängu alustamiseks
            await Task.Delay(500); // Simuleerime töötlemist
            return $"Game started with type '{SelectedGameType}' and configuration '{SelectedGameConfiguration}'!";
        }

        // Mängu uuesti alustamine
        public static async Task<string> StartNewGame()
        {
            SelectedGameType = null;
            SelectedGameConfiguration = null;

            // Loogika uue mängu loomiseks
            await Task.Delay(500); // Simuleerime töötlemist
            return "A new game has been initialized. Please select game type and configuration.";
        }

        // Salvestatud mängu laadimine
        public static async Task<string> LoadGame(string gameName)
        {
            if (string.IsNullOrEmpty(gameName) || !SavedGames.Contains(gameName))
            {
                return "Invalid or non-existing game name.";
            }

            // Loogika mängu laadimiseks
            await Task.Delay(500); // Simuleerime töötlemist
            return $"Game '{gameName}' loaded successfully!";
        }

        // Salvestatud mängude menüü laadimine
        public static List<string> LoadSavedGamesMenu()
        {
            // Võiks lisada loogika mängija nime järgi filtreerimiseks
            return SavedGames.Any() ? SavedGames : new List<string> { "No saved games available." };
        }

        // Mängija nime määramine
        public static void SetPlayerName(string playerName)
        {
            PlayerName = playerName;
        }
    }
}
