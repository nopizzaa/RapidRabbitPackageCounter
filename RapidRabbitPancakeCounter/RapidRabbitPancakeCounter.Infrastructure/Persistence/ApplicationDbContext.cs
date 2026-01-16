using Microsoft.EntityFrameworkCore;
using RapidRabbitPancakeCounter.Application.Common.Interfaces;
using RapidRabbitPancakeCounter.Domain.Common;
using RapidRabbitPancakeCounter.Domain.Entities;

namespace RapidRabbitPancakeCounter.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options), IApplicationDbContext
{
    public DbSet<User> Products => Set<User>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Id generation is handled by the database.
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var idProperty = entityType.FindProperty(nameof(BaseEntity.Id));

            if (idProperty == null || idProperty.ClrType != typeof(Guid)) continue;
            
            idProperty.SetDefaultValueSql("gen_random_uuid()");
            idProperty.ValueGenerated = Microsoft.EntityFrameworkCore.Metadata.ValueGenerated.OnAdd;
        }
        
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username);
        
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // ToDo: Seed initial data for testing later must be removed!
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Username = "test",
                Email = "test@test.com",
                PasswordHash = "test",
                RandomTag = "1TE1",
                Salt = "test",
                HeightInCm = 100,
                CreatedAt = DateTime.UtcNow
            }
        );
    }
    
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Automatically set UpdatedAt for modified entities
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            if (entry.Entity is BaseEntity entity)
            {
                entity.UpdatedAt = DateTime.UtcNow;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
