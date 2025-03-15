using Microsoft.EntityFrameworkCore;
using ICE3.Models;

namespace ICE3.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}
    
    public DbSet<Project> Projects { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Seeding the database
        modelBuilder.Entity<Project>().HasData(
            new Project { ProjectId = 1, Name = "Finish ICE", Description = "Finish ICE today!" }
        );
    }
}