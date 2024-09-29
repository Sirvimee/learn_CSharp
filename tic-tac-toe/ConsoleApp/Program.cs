using GameBrain;
using MenuSystem;

var gameInstance = new TicTacToeBrain(5);

var deepMenu = new Menu(
    EMenuLevel.Deep,
    "TIC-TAC-TOE DEEP", [
        new MenuItem()
        {
            Shortcut = "Y",
            Title = "YYYYYYY",
            MenuItemAction = DummyMethod
        },
    ]
);


var optionsMenu = new Menu(
    EMenuLevel.Secondary,
    "TIC-TAC-TOE Options", [
        new MenuItem()
        {
            Shortcut = "X",
            Title = "X Starts",
            MenuItemAction = deepMenu.Run
        },
        new MenuItem()
        {
            Shortcut = "O",
            Title = "O Starts",
            MenuItemAction = DummyMethod
        },
    ]);

var mainMenu = new Menu(
    EMenuLevel.Main,
    "TIC-TAC-TOE", [
        new MenuItem()
        {
            Shortcut = "O",
            Title = "Options",
            MenuItemAction = optionsMenu.Run
        },
        new MenuItem()
        {
            Shortcut = "N",
            Title = "New game",
            MenuItemAction = NewGame
        }
    ]);

mainMenu.Run();

return;
// ========================================

string DummyMethod()
{
    Console.Write("Just press any key to get out from here! (Any key - as a random choice from keyboard....)");
    Console.ReadKey();
    return "foobar";
}



string NewGame()
{
    ConsoleUI.Visualizer.DrawBoard(gameInstance);

    Console.Write("Give me coordinates <x,y>:");
    var input = Console.ReadLine()!;
    var inputSplit = input.Split(",");
    var inputX = int.Parse(inputSplit[0]);
    var inputY = int.Parse(inputSplit[1]);
    gameInstance.MakeAMove(inputX, inputY);
    
    // loop
    // draw the board again
    // ask input again, validate input
    // is game over?
    
    return "";
}