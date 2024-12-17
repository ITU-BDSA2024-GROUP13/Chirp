using System;
using System.Collections.Generic;
using Chirp.Core.DTO;
using Chirp.Core.Entities;
using Chirp.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Sdk;

namespace Repositories;

public class CheepDBContextTest : IDisposable
{
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Dereference of a possibly null reference.

    private ServiceProvider? _serviceProvider;


    public void Dispose()
    {
        var dbContext = _serviceProvider.GetService<CheepDBContext>();
        dbContext.Database.EnsureDeleted();
    }

    [Fact]
    public async void NoOptions()
    {

        var services = new ServiceCollection();

        // Using In-Memory database for testing: Note that the inmemorydatabse is different from other test classes
        services.AddDbContext<CheepDBContext>();
        services.AddScoped<AuthorRepository>();

        _serviceProvider = services.BuildServiceProvider();

        using (var scope = _serviceProvider.CreateScope())
        {
            // From the scope, get an instance of our database context.
            // Through the `using` keyword, we make sure to dispose it after we are done.
            using var context = scope.ServiceProvider.GetService<CheepDBContext>();
            // Execute the migration from code.
            try
            {
                context.Database.Migrate();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            DbInitializer.SeedDatabase(context);
        }

        using (var scope = _serviceProvider.CreateScope())
        {

            using (var context = scope.ServiceProvider.GetService<CheepDBContext>())
            {
                var repo = new CheepRepository(context);

                List<CheepDTO> list = await repo.ReadPublicMessages(32, 0);
                Assert.True(list.Count > 3);
            }
        }
    }


    [Fact]
    public async void SeedTwice()
    {

        var services = new ServiceCollection();

        // Using In-Memory database for testing: Note that the inmemorydatabse is different from other test classes
        services.AddDbContext<CheepDBContext>();
        services.AddScoped<AuthorRepository>();

        _serviceProvider = services.BuildServiceProvider();

        using (var scope = _serviceProvider.CreateScope())
        {
            // From the scope, get an instance of our database context.
            // Through the `using` keyword, we make sure to dispose it after we are done.
            using var context = scope.ServiceProvider.GetService<CheepDBContext>();
            // Execute the migration from code.
            try
            {
                context.Database.Migrate();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            DbInitializer.SeedDatabase(context);

        }

        using (var scope = _serviceProvider.CreateScope())
        {

            using (var context = scope.ServiceProvider.GetService<CheepDBContext>())
            {
                var repo = new CheepRepository(context);

                List<CheepDTO> list = await repo.ReadPublicMessages(int.MaxValue, 0);
                DbInitializer.SeedDatabase(context);
                List<CheepDTO> list2 = await repo.ReadPublicMessages(int.MaxValue, 0);
                Assert.Equal(list.Count, list2.Count);
            }
        }
    }
}