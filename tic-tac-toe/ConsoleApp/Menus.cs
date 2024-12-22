using MenuSystem;
using DAL;

namespace ConsoleApp
{
    public static class Menus
    {
        private static readonly ConfigRepositoryHardcoded ConfigRepo = new ConfigRepositoryHardcoded();
        // private static readonly GameRepositoryJson GameRepo = new GameRepositoryJson(); // For json
        private static readonly GameRepositoryDb GameRepo = new GameRepositoryDb(); // For database

        public static readonly Menu GameConfigMenu = new Menu(
            EMenuLevel.Deep,
            "Choose Game Config", new List<MenuItem>
            {
                new MenuItem
                {
                    Shortcut = "B",
                    Title = "Big board",
                    MenuItemAction = () =>
                    {
                        var config = ConfigRepo.GetConfigurationByName("Big board");
                        GameController.SetSelectedGameConfiguration(config);
                        return GameController.StartGame();
                    }
                },
                new MenuItem
                {
                    Shortcut = "C",
                    Title = "Classical",
                    MenuItemAction = () =>
                    {
                        var config = ConfigRepo.GetConfigurationByName("Classical");
                        GameController.SetSelectedGameConfiguration(config);
                        return GameController.StartGame();
                    }
                }
            }
        );

        public static readonly Menu GameTypeMenu = new Menu(
            EMenuLevel.Secondary,
            "Choose Game Type", new List<MenuItem>
            {
                new MenuItem
                {
                    Shortcut = "1",
                    Title = "Player vs Player",
                    MenuItemAction = () =>
                    {
                        GameController.SetSelectedGameType("Player vs Player");
                        return GameConfigMenu.Run();
                    }
                },
                new MenuItem
                {
                    Shortcut = "2",
                    Title = "Player vs AI",
                    MenuItemAction = () =>
                    {
                        GameController.SetSelectedGameType("Player vs AI");
                        return GameConfigMenu.Run();
                    }
                },
                new MenuItem
                {
                    Shortcut = "3",
                    Title = "AI vs AI",
                    MenuItemAction = () =>
                    {
                        GameController.SetSelectedGameType("AI vs AI");
                        return GameConfigMenu.Run();
                    }
                }
            }
        );

        public static readonly Menu SavedGamesMenu = new Menu(
            EMenuLevel.Secondary,
            "Choose Saved Game",
            new List<MenuItem>
            {
                new MenuItem
                {
                    Shortcut = "T",
                    Title = "Temporary Placeholder",
                    MenuItemAction = () => "PLACEHOLDER"
                }
            }
        );

        public static readonly Menu MainMenu = new Menu(
            EMenuLevel.Main,
            "Welcome to Tic-Tac-Two!", new List<MenuItem>
            {
                new MenuItem
                {
                    Shortcut = "N",
                    Title = "New Game",
                    MenuItemAction = GameTypeMenu.Run
                }
            }
        );
        
        public static void LoadSavedGamesMenu()
        {
            var savedGames = GameRepo.GetSavedGames(GameController.PlayerName);
            var returnMenuItem = SavedGamesMenu.MenuItems
                .FirstOrDefault(item => item.Shortcut == "R");
            
            SavedGamesMenu.MenuItems.Clear();
            
            SavedGamesMenu.MenuItems.Clear();
            
            if (savedGames.Any())
            {
                int counter = 1; 

                foreach (var gameName in savedGames)
                {
                    SavedGamesMenu.MenuItems.Add(new MenuItem
                    {
                        Shortcut = counter.ToString(), 
                        Title = $"{gameName}", 
                        MenuItemAction = () => GameController.LoadGame(gameName) 
                    });
                    counter++; 
                }

                if (returnMenuItem != null) SavedGamesMenu.MenuItems.Add(returnMenuItem);

                MainMenu.MenuItems.Insert(1, new MenuItem
                {
                    Shortcut = "S",
                    Title = "Saved Games", 
                    MenuItemAction = SavedGamesMenu.Run 
                });
            }
            else
            {
                MainMenu.MenuItems.RemoveAll(item => item.Shortcut == "S");
            }
        }
        
        static Menus()
        {
            LoadSavedGamesMenu(); 
        }
    }
}
