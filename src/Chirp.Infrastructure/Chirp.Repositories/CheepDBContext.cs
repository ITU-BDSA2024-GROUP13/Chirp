using Microsoft.EntityFrameworkCore;
using Chirp.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using NuGet.Protocol.Plugins;


namespace Chirp.Repositories;
public class CheepDBContext : IdentityDbContext<ApplicationUser>
{

    public CheepDBContext(DbContextOptions<CheepDBContext> options) : base(options) { }

    public DbSet<Author> Authors { get; set; }

    public DbSet<Cheep> Cheeps { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=Chat.db");
        }
    }
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
            .HasKey(a => a.AuthorId);

        //Define the relationship betwwen Author and itself (followers)

        modelBuilder.Entity<Author>()
            .HasMany(a => a.Followers)
            .WithMany(a => a.FollowedBy);
            


        // Define the relationship between Cheep and Author
        modelBuilder.Entity<Cheep>()
            .HasOne(c => c.Author)
            .WithMany(a => a.Cheeps)
            .HasForeignKey(c => c.AuthorId);
    }

}