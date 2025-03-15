using Microsoft.EntityFrameworkCore;
using ICE3.Models;

namespace ICE3.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}
    
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectTask> Tasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // One-to-Many relationship between Project and Task
        modelBuilder.Entity<Project>()
            .HasMany(p => p.Tasks)                  // On Project has (potentially) many Tasks
            .WithOne(t => t.Project)             // Each ProjectTask belongs to a Project
            .HasForeignKey(t => t.ProjectId)     // Foreign key in ProjectTask table
            .OnDelete(DeleteBehavior.Cascade);             // Cascade delete ProjectTasks when Project is deleted
        
        // Seeding the database
        modelBuilder.Entity<Project>().HasData(
            new Project { ProjectId = 1, Name = "Finish ICE", Description = "Finish ICE today!" }
        );
    }
}