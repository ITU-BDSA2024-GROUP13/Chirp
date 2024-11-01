using Chirp.Core.DTO;
using Chirp.Core.Entities;
using Chirp.Repositories;
using Chirp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Sdk;

namespace Repositories;

public class CheepServiceTest : IDisposable
{
    #pragma warning disable CS8602 // Dereference of a possibly null reference.
    #pragma warning disable CS8604 // Dereference of a possibly null reference.
    #pragma warning disable CS8618 
    private ServiceProvider _serviceProvider;
    private CheepService _cheepService;
    private CheepRepository _cheepRepository;
    private AuthorRepository _authorRepository;
    

    public CheepServiceTest(){

        var services = new ServiceCollection();

        // Using In-Memory database for testing
        services.AddDbContext<CheepDBContext>(options =>
            options.UseInMemoryDatabase("TestDbService"));

        services.AddScoped<CheepRepository>();
        services.AddScoped<AuthorRepository>();
        services.AddScoped<ICheepService, CheepService>();

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

            var context = scope.ServiceProvider.GetService<CheepDBContext>();
            _cheepRepository = new CheepRepository(context);
            _authorRepository = new AuthorRepository(context);
            _cheepService = new CheepService(_cheepRepository, _authorRepository);


            List<CheepDTO> list = await _cheepService.ReadPublicMessages(0);
            Assert.True(list.Count > 3);

        }
     
    }

   

}