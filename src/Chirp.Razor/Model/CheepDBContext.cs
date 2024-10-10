using Microsoft.EntityFrameworkCore;

namespace Chirp.Razor.Model;

public class CheepDBContext : DbContext
{
    public DbSet<Author> Authors { get; set; }
    public DbSet<Cheep> Cheeps { get; set; }

    public CheepDBContext(DbContextOptions<CheepDBContext> options) : base(options)
    {
    }
}