namespace DAL;

public class FileHelper
{
    public static string BasePath = Environment
                                        .GetFolderPath(System.Environment.SpecialFolder.UserProfile)
                                    + Path.DirectorySeparatorChar + "tic-tac-two" + Path.DirectorySeparatorChar;
}