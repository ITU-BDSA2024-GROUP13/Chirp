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

    [Fact]
    public async void ReadPublicMessages()
    {
        using (var scope = _serviceProvider.CreateScope()){

            using (var context = scope.ServiceProvider.GetService<CheepDBContext>()){
                var repo = new CheepRepository(context);

                List<CheepDTO> list = await repo.ReadPublicMessages(32, 0);
                string authorName = list[0].Author;
                Boolean otherAuthor = false;


                foreach (CheepDTO item in list)
                {
                    if(!item.Author.Equals(authorName)){
                        otherAuthor = true;
                    }
                }
                // Should not be larger than the take value
                Assert.False(list.Count > 32);
                // The most recent message in the test db
                Assert.Equal("Starbuck now is what we hear the worst.", list[0].Text);
                Assert.True(otherAuthor);
            }
        }
    }

       [Fact]
    public async void ReadUserMessages()
    {
        using (var scope = _serviceProvider.CreateScope()){

            using (var context = scope.ServiceProvider.GetService<CheepDBContext>()){
                var repo = new CheepRepository(context);

                List<CheepDTO> list = await repo.ReadUserMessages("Helge", 32, 0);
                string authorName = list[0].Author;
                Boolean otherAuthor = false;


                foreach (CheepDTO item in list)
                {
                    if(!item.Author.Equals(authorName)){
                        otherAuthor = true;
                    }
                }
                // Should not be larger than the take value
                Assert.False(list.Count > 32);
                // The most recent message in the test db
                Assert.False(otherAuthor);
                // The authors timeline should not be empty
                Assert.True(list.Count > 0);
            }
        }
    }



}