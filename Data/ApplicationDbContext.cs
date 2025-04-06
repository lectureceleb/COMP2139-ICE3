using Microsoft.EntityFrameworkCore;
using ICE3.Models;
using ICE3.Areas.ProjectManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ICE3.Data;

public class ApplicationDbContext : IdentityDbContext // <ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectTask> Tasks { get; set; }
    public DbSet<ProjectComment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Ensure Identity configurations and tables are created
        base.OnModelCreating(modelBuilder);
        // modelBuilder.HasDefaultSchema("ICE3");   // Needed???
        
        // One-to-Many relationship between Project and Task
        modelBuilder.Entity<Project>()
            .HasMany(p => p.Tasks)                  // One Project has (potentially) many Tasks
            .WithOne(t => t.Project)             // Each ProjectTask belongs to a Project
            .HasForeignKey(t => t.ProjectId)     // Foreign key in ProjectTask table
            .OnDelete(DeleteBehavior.Cascade);             // Cascade delete ProjectTasks when Project is deleted
        
        // Seeding the database
        modelBuilder.Entity<Project>().HasData(
            new Project { ProjectId = 1, Name = "Finish ICE", Description = "Finish ICE today!" }
        );
/*
        // Refactoring Identity table names
        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.ToTable("Users");
        });
        
        modelBuilder.Entity<IdentityRole>(entity =>
        {
            entity.ToTable("Roles");
        });
        modelBuilder.Entity<IdentityUserRole<string>>(entity =>
        {
            entity.ToTable("UserRoles");
        });
        modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
        {
            entity.ToTable("UserClaims");
        });
        modelBuilder.Entity<IdentityUserToken<string>>(entity =>
        {
            entity.ToTable("UserTokens");
        });
        modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
        {
            entity.ToTable("UserLogins");
        });
        modelBuilder.Entity<IdentityRoleClaim<string>>(entity =>
        {
            entity.ToTable("RoleClaims");
        });
        */
    }
}