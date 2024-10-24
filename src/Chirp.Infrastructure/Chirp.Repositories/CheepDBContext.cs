using Chirp.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Repositories;

public class CheepDBContext : DbContext
{
    public DbSet<Author> Authors { get; set; }
    public DbSet<Cheep> Cheeps { get; set; }

    public CheepDBContext(DbContextOptions<CheepDBContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder){

        modelBuilder.Entity<Cheep>()
        .HasKey(c => new { c.CheepId});
        modelBuilder.Entity<Author>()
        .HasKey(a => new { a.AuthorId});
    }
    

}