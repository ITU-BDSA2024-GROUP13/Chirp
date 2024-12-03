using Microsoft.EntityFrameworkCore;
using Chirp.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Chirp.Repositories;

/// <summary>
/// Represents the database context for the Chirp application, providing access to the Authors and Cheeps tables.
/// </summary>
public class CheepDBContext(DbContextOptions<CheepDBContext> options) : IdentityDbContext<Author>(options)
{
    /// <summary>
    /// Gets or sets the Authors table in the database.
    /// </summary>
    public required DbSet<Author> Authors { get; set; }

    /// <summary>
    /// Gets or sets the Cheeps table in the database.
    /// </summary>
    public required DbSet<Cheep> Cheeps { get; set; }
    
    /// <summary>
    /// Configures the database connection and options.
    /// </summary>
    /// <param name="optionsBuilder">The options builder to configure the context.</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=Chat.db");
        }
    }

    /// <summary>
    /// Configures the entity relationships and model properties for the context.
    /// </summary>
    /// <param name="modelBuilder">The model builder to configure the database schema.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Your custom configurations for the Cheep entity
        modelBuilder.Entity<Cheep>()
            .HasKey(c => c.CheepId);

        modelBuilder.Entity<Cheep>()
            .Property(c => c.Text)
            .IsRequired()
            .HasMaxLength(160);

        // Your custom configurations for the Author entity
        modelBuilder.Entity<Author>()
            .HasKey(a => a.Id);

        modelBuilder.Entity<Author>()
            .Property(f => f.Id)
            .ValueGeneratedOnAdd();

        //Define the relationship betwwen Author and itself (followers)

        modelBuilder.Entity<Author>()
            .HasMany(a => a.Followers)
            .WithMany(a => a.FollowedBy);

        // Define the relationship between Cheep and Author
        modelBuilder.Entity<Cheep>()
            .HasOne(c => c.Author)
            .WithMany(a => a.Cheeps)
            .HasForeignKey(c => c.AuthorId);

        // Define the relationship of Likes
        modelBuilder.Entity<Cheep>()
            .HasMany(c => c.Likes)
            .WithMany(a => a.LikedCheeps);

        // Define the relationship of Dislikes
        modelBuilder.Entity<Cheep>()
            .HasMany(c => c.Dislikes)
            .WithMany(a => a.DislikedCheeps);
    }

}