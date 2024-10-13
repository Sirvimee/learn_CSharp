using MenuSystem;
using GameBrain;
using DAL;

namespace ConsoleApp
{
    public static class Menus
    {
        private static readonly ConfigRepository ConfigRepo = new ConfigRepository();

        public static readonly Menu GameConfigMenu = new Menu(
            EMenuLevel.Deep,
            "Choose Game Configuration", new List<MenuItem>
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
            });

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
            });

        public static readonly Menu MainMenu = new Menu(
            EMenuLevel.Main,
            "Welcome to Tic-Tac-Two!", new List<MenuItem>
            {
                new MenuItem
                {
                    Shortcut = "N",
                    Title = "New Game",
                    MenuItemAction = GameTypeMenu.Run
                },
                new MenuItem
                {
                    Shortcut = "S",
                    Title = "Saved Game",
                    MenuItemAction = DummyMethod // TODO Replace with actual saved game loading method
                }
            });

        private static string DummyMethod()
        {
            Console.Write("Just press any key to get out from here! (Any key - as a random choice from keyboard....)");
            Console.ReadKey();
            return "foobar";
        }
    }
}