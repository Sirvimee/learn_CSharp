using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class AppDbContext : DbContext
{
    public DbSet<Config> Configs { get; set; }
    public DbSet<Game> Games { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Config>().HasData(
            new Config
            {
                Id = 1,
                Name = "Classical",
                BoardSizeWidth = 5,
                BoardSizeHeight = 5,
                GridSizeHeight = 3,
                GridSizeWidth = 3,
                WinCondition = 3,
                MovePieceAfterNMoves = 2
            },
            new Config 
            {
                Id = 2,
                Name = "Big board",
                BoardSizeWidth = 10,
                BoardSizeHeight = 10,
                GridSizeHeight = 4,
                GridSizeWidth = 4,
                WinCondition = 4,
                MovePieceAfterNMoves = 2
            }
        );
    }

}