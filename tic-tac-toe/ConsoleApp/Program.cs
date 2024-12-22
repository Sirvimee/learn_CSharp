using ConsoleApp;
using GameBrain;

Console.WriteLine("What is your player name? ");
string? playerName = Console.ReadLine();
GameController.PlayerName = playerName;

// menu configuration is in Menus.cs
Menus.MainMenu.Run();