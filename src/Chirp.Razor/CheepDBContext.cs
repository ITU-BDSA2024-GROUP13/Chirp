using Microsoft.EntityFrameworkCore;

namespace Chirp.Razor;

public class CheepDBContext : DbContext
{
    public DbSet<Author> authors { get; set; }
    public DbSet<Cheep> cheeps { get; set; }

    public CheepDBContext(DbContextOptions<CheepDBContext> options) : base(options)
    {
    }
}