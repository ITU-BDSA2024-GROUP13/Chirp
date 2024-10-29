using Chirp.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Repositories;
using Chirp.Core.Entities;


public class CheepDBContext(DbContextOptions<CheepDBContext> options) : DbContext(options)
{
    public DbSet<Author> Authors { get; set; }
    public DbSet<Cheep> Cheeps { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Cheep>()
        .HasKey(a => new { a.CheepId });
        

        modelBuilder.Entity<Cheep>()
        .Property(t => t.Text)
        .IsRequired()
        .HasMaxLength(160);

        modelBuilder.Entity<Author>()
        .HasKey(a => new { a.AuthorId });
    }


}