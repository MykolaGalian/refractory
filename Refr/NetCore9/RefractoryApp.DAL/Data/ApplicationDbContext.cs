using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RefractoryApp.DAL.Identity;
using RefractoryApp.DAL.Models;

namespace RefractoryApp.DAL.Data;

public class ApplicationDbContext : IdentityDbContext<
    ApplicationUser, 
    ApplicationRole, 
    int, 
    ApplicationUserClaim, 
    ApplicationUserRole, 
    ApplicationUserLogin, 
    ApplicationRoleClaim, 
    ApplicationUserToken>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<UserInfo> UserInfos { get; set; }
    public DbSet<Refractory> Refractories { get; set; }
    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // UserInfo configuration - связь один к одному с ApplicationUser
        modelBuilder.Entity<UserInfo>(entity =>
        {
            entity.HasIndex(e => e.Login).IsUnique();
            
            // UserInfo.Id = ApplicationUser.Id (как в старом проекте)
            entity.HasOne(u => u.ApplicationUser)
                .WithOne(a => a.UserInfo)
                .HasForeignKey<UserInfo>(u => u.Id);
        });

        // Refractory configuration - предотвращение каскадного удаления как в старом проекте
        modelBuilder.Entity<Refractory>(entity =>
        {
            entity.HasOne(r => r.UserInfo)
                .WithMany(u => u.Refractories)
                .HasForeignKey(r => r.UserInfoId)
                .OnDelete(DeleteBehavior.Restrict); // WillCascadeOnDelete(false) из старого проекта
        });

        // Comment configuration
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasOne(c => c.UserInfo)
                .WithMany()
                .HasForeignKey(c => c.UserInfoId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(c => c.Refractory)
                .WithMany(r => r.Comments)
                .HasForeignKey(c => c.RefractoryId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}