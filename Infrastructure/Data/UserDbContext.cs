using Customization_Management_API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Customization_Management_API.Infrastructure.Data;

public class UserDbContext( DbContextOptions<UserDbContext> options ) : DbContext( options )
{
    public DbSet<Unit> Units{ get; set; }
    public DbSet<Customization> Customizations{ get; set; }
    public DbSet<CustomizationRequest> CustomizationRequests{ get; set; }

    protected override void OnModelCreating( ModelBuilder modelBuilder )
    {
        base.OnModelCreating( modelBuilder );

        modelBuilder.Entity<Unit>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.DevelopmentName).IsRequired();
            entity.Property(e => e.UnitNumber).IsRequired();
            entity.Property(e => e.ClientName).IsRequired();
            entity.Property(e => e.ClientCPF).IsRequired();
        });

        modelBuilder.Entity<Customization>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.Description).IsRequired();
            entity.Property(e => e.Price).HasPrecision(18, 2);
            entity.HasIndex(e => e.Name).IsUnique();
        });

        modelBuilder.Entity<CustomizationRequest>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Unit)
                .WithMany(u => u.CustomizationRequests)
                .HasForeignKey(e => e.UnitId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasMany(e => e.Customizations)
                .WithMany(c => c.CustomizationRequests);
            
            entity.Property(e => e.TotalValue).HasPrecision(18, 2);
        });
    }
} 