// See https://aka.ms/new-console-template for more information

using MenuSystem;

var mainMenu = new Menu("TIC-TAC-TOE", [
    new MenuItem()
    {
        Shortcut = "O",
        Title = "Options",
    },

    new MenuItem()
    {
        Shortcut = "N",
        Title = "New game",
    }

]);

mainMenu.Run();

return;
// ======================================

static void MenuMain()
{
    MenuStart();
    
    Console.WriteLine("O) Options");
    Console.WriteLine("N) New game");
    Console.WriteLine("L) Load game");
    Console.WriteLine("E) Exit");

    MenuEnd();
}

static void MenuOptions()
{
    MenuStart();
    
    Console.WriteLine("Choose symbol for player one (X)");
    Console.WriteLine("Choose symbol for player two (O)");
    
    MenuEnd();
}

static void MenuStart()
{
    Console.WriteLine("TIC-TAC-TOE");
    Console.WriteLine("=======================");
}

static void MenuEnd()
{
    Console.WriteLine();
    Console.Write(">");
}


