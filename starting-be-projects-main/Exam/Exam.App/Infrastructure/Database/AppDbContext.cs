using Exam.App.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Exam.App.Infrastructure.Database;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Project> Projects { get; set; }
    public DbSet<Skill> Skills { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Project>()
            .HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(p => p.UserId);

        // Seed Roles
        modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole { Id = "b1c4c3f8-4d2f-4e62-9a4b-8e3a2a9e8a01", Name = "Administrator", NormalizedName = "ADMINISTRATOR" },
            new IdentityRole { Id = "1bfe3e46-2f8f-4a9c-bb7b-2f0d8c2e6d02", Name = "User", NormalizedName = "USER" }
        );
    }
}
