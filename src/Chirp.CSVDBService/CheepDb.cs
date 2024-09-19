using Chirp.CLI.Client;
using Microsoft.EntityFrameworkCore;

class CheepDb : DbContext
{
    public CheepDb(DbContextOptions<CheepDb> options)
        : base(options) { }

    public DbSet<Cheep> Todos => Set<Cheep>();
}