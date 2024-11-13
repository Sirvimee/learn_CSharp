using Domain;

namespace DAL;

public static class DbSeeder
{
    public static void Seed(AppDbContext dbContext)
    {
        if (!dbContext.Configs.Any(c => c.Name == "Classical"))
        {
            var classicalConfig = new Config
            {
                Name = "Classical",
                BoardSizeWidth = 5,
                BoardSizeHeight = 5,
                GridSizeHeight = 3,
                GridSizeWidth = 3,
                WinCondition = 3,
                MovePieceAfterNMoves = 2
            };

            dbContext.Configs.Add(classicalConfig);
            dbContext.SaveChanges();
        }
        
        if (!dbContext.Configs.Any(c => c.Name == "Big board"))
        {
            var bigBoardConfig = new Config
            {
                Name = "Big board",
                BoardSizeWidth = 10,
                BoardSizeHeight = 10,
                GridSizeHeight = 4,
                GridSizeWidth = 4,
                WinCondition = 4,
                MovePieceAfterNMoves = 2
            };

            dbContext.Configs.Add(bigBoardConfig);
            dbContext.SaveChanges();
        }
    }
}
