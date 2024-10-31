using Chirp.Core.DTO;
using Chirp.Core.Entities;
using Chirp.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit.Sdk;

namespace Repositories;

public class CheepRepositoryTest : IDisposable
{

    private ServiceProvider _serviceProvider;
    

    public CheepRepositoryTest(){

        var services = new ServiceCollection();

        // Using In-Memory database for testing
        services.AddDbContext<CheepDBContext>(options =>
            options.UseInMemoryDatabase("TestDb"));

        services.AddScoped<CheepRepository>();

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
    }

    public void Dispose()
    {
        var dbContext = _serviceProvider.GetService<CheepDBContext>();
        dbContext.Database.EnsureDeleted();
    }

    [Fact]
    public async void SeededDatabase()
    {
        using (var scope = _serviceProvider.CreateScope()){

            using (var context = scope.ServiceProvider.GetService<CheepDBContext>()){
                var repo = new CheepRepository(context);

                List<CheepDTO> list = await repo.ReadPublicMessages(32, 0);
                Assert.True(list.Count > 3);

            }
        }
    }

}