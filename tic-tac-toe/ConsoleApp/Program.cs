using MenuSystem;

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
            MenuItemAction = DummyMethod
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