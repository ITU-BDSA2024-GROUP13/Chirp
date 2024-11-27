using Chirp.Core.DTO;
using Chirp.Core.Entities;
using Chirp.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Sdk;

namespace Repositories;

public class AuthorRepositoryTest : IDisposable
{
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Dereference of a possibly null reference.

    private ServiceProvider _serviceProvider;


    public AuthorRepositoryTest()
    {

        var services = new ServiceCollection();

        // Using In-Memory database for testing: Note that the inmemorydatabse is different from other test classes
        services.AddDbContext<CheepDBContext>(options =>
            options.UseInMemoryDatabase("TestDb2"));
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
    }

    public void Dispose()
    {
        var dbContext = _serviceProvider.GetService<CheepDBContext>();
        dbContext.Database.EnsureDeleted();
    }

    [Fact]
    public async void SeededDatabase()
    {
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
    public async void FindAuthorByName()
    {
        using (var scope = _serviceProvider.CreateScope())
        {

            using (var context = scope.ServiceProvider.GetService<CheepDBContext>())
            {
                var repo = new AuthorRepository(context);

                List<AuthorDTO> authors = await repo.FindAuthorByName("Helge");

                Assert.False(authors.Count > 1);
                Assert.Equal("Helge", authors[0].Name);
            }
        }
    }

    [Fact]
    public async void FindMultipleAuthors()
    {
        using (var scope = _serviceProvider.CreateScope())
        {

            using (var context = scope.ServiceProvider.GetService<CheepDBContext>())
            {
                var repo = new AuthorRepository(context);

                List<AuthorDTO> authors = await repo.FindAuthorByName("J");

                Assert.Equal("Jacqualine Gilcoine", authors[0].Name);
                Assert.Equal("Johnnie Calixto", authors[1].Name);
            }
        }
    }

    [Fact]
    public async void FindAuthorByEmail()
    {
        using (var scope = _serviceProvider.CreateScope())
        {

            using (var context = scope.ServiceProvider.GetService<CheepDBContext>())
            {
                var repo = new AuthorRepository(context);

                List<AuthorDTO> authors = await repo.FindAuthorByEmail("ropf@itu.dk");

                Assert.False(authors.Count > 1);
                Assert.Equal("Helge", authors[0].Name);
            }
        }
    }

    [Fact]
    public async void FindMultipleAuthorsByEmail()
    {
        using (var scope = _serviceProvider.CreateScope())
        {

            using (var context = scope.ServiceProvider.GetService<CheepDBContext>())
            {
                var repo = new AuthorRepository(context);

                List<AuthorDTO> authors = await repo.FindAuthorByEmail("J");

                Assert.Equal("Jacqualine Gilcoine", authors[0].Name);
                Assert.Equal("Johnnie Calixto", authors[1].Name);
            }
        }
    }

    [Fact]
    public async void CreateAuthor()
    {
        using (var scope = _serviceProvider.CreateScope())
        {

            using (var context = scope.ServiceProvider.GetService<CheepDBContext>())
            {
                var repo = new AuthorRepository(context);

                AuthorDTO newAuthor = new() { Name = "Helge Helgesen", Email = "Helge@gmail.com" };
                await repo.CreateAuthor(newAuthor);

                List<AuthorDTO> authors = await repo.FindAuthorByName("Helge");
                Assert.Equal("Helge", authors[0].Name);
                Assert.Equal("Helge Helgesen", authors[1].Name);
            }
        }
    }

  [Fact]
    public async void FindAuthors()
    {
        using (var scope = _serviceProvider.CreateScope())
        {

            using (var context = scope.ServiceProvider.GetService<CheepDBContext>())
            {
                var repo = new AuthorRepository(context);

                AuthorDTO newAuthor = new() { Name = "Helge Helgesen", Email = "Helge@gmail.com" };
                List<AuthorDTO> list = await repo.FindAuthors("J", 2);

                Assert.Equal(2, list.Count);
                List<AuthorDTO> list2 = await repo.FindAuthors("J", 3);
                Assert.Equal("Malcolm Janski", list2[2].Name);
            }
        }
    }

    [Fact]
    public async void FindSpecificAuthorByName()
    {
        using (var scope = _serviceProvider.CreateScope())
        {

            using (var context = scope.ServiceProvider.GetService<CheepDBContext>())
            {
                var repo = new AuthorRepository(context);

                AuthorDTO newAuthor = new() { Name = "Helge Helgesen", Email = "Helge@gmail.com" };
                await repo.CreateAuthor(newAuthor);

                AuthorDTO author = await repo.FindSpecificAuthorByName("Helge");
                Assert.Equal("Helge", author.Name);
            }
        }
    }

    [Fact]
    public async void FindSpecificAuthorById()
    {
        using (var scope = _serviceProvider.CreateScope())
        {

            using (var context = scope.ServiceProvider.GetService<CheepDBContext>())
            {
                var repo = new AuthorRepository(context);

                AuthorDTO author = await repo.FindSpecificAuthorById("1");
                Assert.Equal("Roger Histand", author.Name);
            }
        }
    }

        [Fact]
    public async void FindSpecificAuthorByEmail()
    {
        using (var scope = _serviceProvider.CreateScope())
        {

            using (var context = scope.ServiceProvider.GetService<CheepDBContext>())
            {
                var repo = new AuthorRepository(context);

                AuthorDTO author = await repo.FindSpecificAuthorByEmail("adho@itu.dk");
                Assert.Equal("Adrian", author.Name);
            }
        }
    }

    [Fact]
    public async void AddFollower()
    {
        using (var scope = _serviceProvider.CreateScope())
        {

            using (var context = scope.ServiceProvider.GetService<CheepDBContext>())
            {
                var repo = new AuthorRepository(context);

                await repo.AddFollower("1", "12");

                List<AuthorDTO> list = (List<AuthorDTO>)await repo.GetFollowers("Roger Histand");
                List<AuthorDTO> list2 = (List<AuthorDTO>)await repo.GetFollowedby("Adrian");

                Assert.Equal("Adrian", list[0].Name);
                Assert.Equal("Roger Histand", list2[0].Name);
            }
        }
    }

    [Fact]

    public async void RemoveFollower()
    {
        using (var scope = _serviceProvider.CreateScope())
        {

            using (var context = scope.ServiceProvider.GetService<CheepDBContext>())
            {
                var repo = new AuthorRepository(context);

                await repo.AddFollower("1", "12");

                List<AuthorDTO> list = (List<AuthorDTO>)await repo.GetFollowers("Roger Histand");
                Assert.Equal("Adrian", list[0].Name);

                await repo.RemoveFollower("1", "12");
                List<AuthorDTO> list2 = (List<AuthorDTO>)await repo.GetFollowers("Roger Histand");
                Assert.False(list2.Any());
            }
        }
    }

    [Fact]

    public async void RemoveFollowerException()
    {
        using (var scope = _serviceProvider.CreateScope())
        {

            using (var context = scope.ServiceProvider.GetService<CheepDBContext>())
            {
                var repo = new AuthorRepository(context);
                await Assert.ThrowsAsync<InvalidDataException>(async () => await repo.RemoveFollower("1", "12"));
            }
        }
    }


    [Fact]
    public async void RemoveAllFollowers()
    {
        using (var scope = _serviceProvider.CreateScope())
        {

            using (var context = scope.ServiceProvider.GetService<CheepDBContext>())
            {
                var repo = new AuthorRepository(context);

                await repo.AddFollower("1", "12");
                await repo.AddFollower("1", "3");
                await repo.AddFollower("1", "4");


                List<AuthorDTO> list = (List<AuthorDTO>)await repo.GetFollowers("Roger Histand");
                Assert.NotEmpty(list);

                await repo.RemoveAllFollowers("1");
                List<AuthorDTO> list2 = (List<AuthorDTO>)await repo.GetFollowers("Roger Histand");
                Assert.Empty(list2);
            }
        }
    }

    [Fact]
    public async void RemoveAllFollowedby()
    {
        using (var scope = _serviceProvider.CreateScope())
        {

            using (var context = scope.ServiceProvider.GetService<CheepDBContext>())
            {
                var repo = new AuthorRepository(context);

                await repo.AddFollower("12", "1");
                await repo.AddFollower("11", "1");
                await repo.AddFollower("8", "1");


                List<AuthorDTO> list = (List<AuthorDTO>)await repo.GetFollowedby("Roger Histand");
                Assert.NotEmpty(list);

                await repo.RemoveAllFollowedby("1");
                List<AuthorDTO> list2 = (List<AuthorDTO>)await repo.GetFollowedby("Roger Histand");
                Assert.Empty(list2);
            }
        }
    }

    [Fact]
    public async void RemoveAuthor()
    {
        using (var scope = _serviceProvider.CreateScope())
        {

            using (var context = scope.ServiceProvider.GetService<CheepDBContext>())
            {
                var repo = new AuthorRepository(context);

                await repo.RemoveAuthor("11");

                await Assert.ThrowsAsync<NullReferenceException>(async () => await repo.FindSpecificAuthorByName("Helge"));
            }
        }
    }



}