// See https://aka.ms/new-console-template for more information

using DAL;
using Microsoft.EntityFrameworkCore;

var connectionString = $"Data Source={FileHelper.BasePath}app.db";
    
Console.WriteLine("Hello, DB/EF demo!");
Console.WriteLine("Db is in: " + connectionString);

var contextOptions = new DbContextOptionsBuilder<AppDbContext>()
    .UseSqlite(connectionString)
    .EnableDetailedErrors()
    .EnableSensitiveDataLogging()
    .Options;

using var ctx = new AppDbContext(contextOptions);
ctx.Database.Migrate();

Console.WriteLine($"Games in db {ctx.SaveGames.Count()}");
Console.WriteLine($"Configs in db {ctx.Configurations.Count()}");

foreach (var conf in ctx.Configurations
             .Include(c => c.SaveGames)
             .Where(c => c.Id == 2))
{
    Console.WriteLine(conf);
}

