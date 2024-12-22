using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class AppDbContext : DbContext
{
    public DbSet<Configuration> Configurations { get; set; } // see tuleb teha mudelist (Domain kaustas)
    public DbSet<SaveGame> SaveGames { get; set; } // see tuleb teha mudelist (Domain kaustas)
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

}