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


    public CheepServiceTest()
    {

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
        using (var scope = _serviceProvider.CreateScope())
        {

            var context = scope.ServiceProvider.GetService<CheepDBContext>();
            _cheepRepository = new CheepRepository(context);
            _authorRepository = new AuthorRepository(context);
            _cheepService = new CheepService(_cheepRepository, _authorRepository);


            List<CheepDTO> list = await _cheepService.ReadPublicMessages(0);
            Assert.True(list.Count > 3);

        }

    }

    [Fact]
    public async void ReadPublicMessages()
    {
        using (var scope = _serviceProvider.CreateScope())
        {

            var context = scope.ServiceProvider.GetService<CheepDBContext>();
            _cheepRepository = new CheepRepository(context);
            _authorRepository = new AuthorRepository(context);
            _cheepService = new CheepService(_cheepRepository, _authorRepository);


            List<CheepDTO> list = await _cheepService.ReadPublicMessages(0);
            string authorName = list[0].Author;
            Boolean otherAuthor = false;

            foreach (CheepDTO item in list)
            {
                if (!item.Author.Equals(authorName))
                {
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

    [Fact]
    public async void ReadUserMessages()
    {
        using (var scope = _serviceProvider.CreateScope())
        {

            var context = scope.ServiceProvider.GetService<CheepDBContext>();
            _cheepRepository = new CheepRepository(context);
            _authorRepository = new AuthorRepository(context);
            _cheepService = new CheepService(_cheepRepository, _authorRepository);


            List<CheepDTO> list = await _cheepService.ReadUserMessages("Helge", 0);
            string authorName = list[0].Author;
            Boolean otherAuthor = false;


            foreach (CheepDTO item in list)
            {
                if (!item.Author.Equals(authorName))
                {
                    otherAuthor = true;
                    break;
                }
            }
            // Should not be larger than the take value
            Assert.False(list.Count > 32);
            // Should not show other authors on the timeline
            Assert.False(otherAuthor);
            // The authors timeline should not be empty
            Assert.True(list.Count > 0);
            Assert.Equal("Helge", authorName);

        }

    }

    [Fact]
    public async void CreateMessage()
    {
        using (var scope = _serviceProvider.CreateScope())
        {

            var context = scope.ServiceProvider.GetService<CheepDBContext>();
            _cheepRepository = new CheepRepository(context);
            _authorRepository = new AuthorRepository(context);
            _cheepService = new CheepService(_cheepRepository, _authorRepository);

            bool messageCreated = false;

            List<CheepDTO> prevList = await _cheepService.ReadUserMessages("Helge", 0);

            CheepDTO newMessage = new() { Author = "Helge", AuthorId = "11", Text = "I love group 13!", Timestamp = 12345, Likes = 0 };

            await _cheepRepository.CreateMessage(newMessage);

            List<CheepDTO> newList = await _cheepService.ReadUserMessages("Helge", 0);


            foreach (CheepDTO item in newList)
            {
                if (item.Text.Equals(newMessage.Text))
                {
                    messageCreated = true;
                    break;
                }
            }
            Assert.True(newList.Count > prevList.Count);
            Assert.True(messageCreated);

        }

    }

    [Fact]
    public async void CreateMessageFail()
    {
        using (var scope = _serviceProvider.CreateScope())
        {

            var context = scope.ServiceProvider.GetService<CheepDBContext>();
            _cheepRepository = new CheepRepository(context);
            _authorRepository = new AuthorRepository(context);
            _cheepService = new CheepService(_cheepRepository, _authorRepository);

            bool messageCreated = false;

            List<CheepDTO> prevList = await _cheepService.ReadUserMessages("Helge", 0);

            CheepDTO newMessage = new()
            {
                Author = "Helge",
                AuthorId = "11",
                Text = "I love group 13! " +
            "I love group 13! I love group 13! I love group 13! I love group 13! I love group 13! I love group 13! I love group 13! I love group 13! I love !",
                Timestamp = 12345,
                Likes = 0
            };

            await _cheepService.CreateMessage(newMessage);

            List<CheepDTO> newList = await _cheepService.ReadUserMessages("Helge", 0);


            foreach (CheepDTO item in newList)
            {
                if (item.Text.Equals(newMessage.Text))
                {
                    messageCreated = true;
                    break;
                }
            }
            Assert.True(newList.Count == prevList.Count);
            Assert.False(messageCreated);

        }

    }


    [Fact]
    public async void CreateAuthorFromNewMessage()
    {
        using (var scope = _serviceProvider.CreateScope())
        {

            var context = scope.ServiceProvider.GetService<CheepDBContext>();
            _cheepRepository = new CheepRepository(context);
            _authorRepository = new AuthorRepository(context);
            _cheepService = new CheepService(_cheepRepository, _authorRepository);

            bool messageCreated = false;

            CheepDTO newMessage = new() { Author = "Helge2", AuthorId = "13", Text = "I love group 13!", Timestamp = 12345, Likes = 0 };

            await _cheepService.CreateMessage(newMessage);

            AuthorDTO author = await _cheepService.FindSpecificAuthorByName("Helge2");

            List<CheepDTO> newList = await _cheepService.ReadUserMessages("Helge2", 0);


            foreach (CheepDTO item in newList)
            {
                if (item.Text.Equals(newMessage.Text))
                {
                    messageCreated = true;
                    break;
                }
            }

            Assert.Equal("Helge2", author.Name);

            Assert.True(messageCreated);

        }

    }

    [Fact]
    public async void CreateAuthorFromNewMessageAlternative()
    {
        using (var scope = _serviceProvider.CreateScope())
        {

            var context = scope.ServiceProvider.GetService<CheepDBContext>();
            _cheepRepository = new CheepRepository(context);
            _authorRepository = new AuthorRepository(context);
            _cheepService = new CheepService(_cheepRepository, _authorRepository);

            bool messageCreated = false;

            List<CheepDTO> prevList = await _cheepService.ReadUserMessages("Helg", 0);

            CheepDTO newMessage = new() { Author = "Helg", AuthorId = "13", Text = "I love group 13!", Timestamp = 12345, Likes = 0 };

            await _cheepService.CreateMessage(newMessage);

            List<CheepDTO> newList = await _cheepService.ReadUserMessages("Helg", 0);


            foreach (CheepDTO item in newList)
            {
                if (item.Text.Equals(newMessage.Text))
                {
                    messageCreated = true;
                    break;
                }
            }

            Assert.True(newList.Count > prevList.Count);
            Assert.True(messageCreated);

        }
    }

    [Fact]
    public async void Follow()
    {
        using (var scope = _serviceProvider.CreateScope())
        {

            var context = scope.ServiceProvider.GetService<CheepDBContext>();
            _cheepRepository = new CheepRepository(context);
            _authorRepository = new AuthorRepository(context);
            _cheepService = new CheepService(_cheepRepository, _authorRepository);


            await _cheepService.Follow("1", "12");


            List<AuthorDTO> list = await _cheepService.GetFollowers("Roger Histand");
            List<AuthorDTO> list2 = await _cheepService.GetFollowersbyId("1");
            List<AuthorDTO> list3 = await _cheepService.GetFollowedby("Adrian");
            List<AuthorDTO> list4 = await _cheepService.GetFollowedbybyId("12");


            Assert.Equal("Adrian", list[0].Name);
            Assert.Equal("Adrian", list2[0].Name);
            Assert.Equal("Roger Histand", list3[0].Name);
            Assert.Equal("Roger Histand", list4[0].Name);


        }

    }

    [Fact]
    public async void IsFollowing()
    {
        using (var scope = _serviceProvider.CreateScope())
        {

            var context = scope.ServiceProvider.GetService<CheepDBContext>();
            _cheepRepository = new CheepRepository(context);
            _authorRepository = new AuthorRepository(context);
            _cheepService = new CheepService(_cheepRepository, _authorRepository);


            await _cheepService.Follow("1", "12");

            bool isfollowing = await _cheepService.IsFollowing("1", "12");
            Assert.True(isfollowing);
            bool isfollowing2 = await _cheepService.IsFollowing("1", "11");
            Assert.False(isfollowing2);

            bool isfollowing3 = await _cheepService.IsFollowing("12", "1");
            Assert.False(isfollowing3);
        }

    }

    [Fact]
    public async void Unfollow()
    {
        using (var scope = _serviceProvider.CreateScope())
        {

            var context = scope.ServiceProvider.GetService<CheepDBContext>();
            _cheepRepository = new CheepRepository(context);
            _authorRepository = new AuthorRepository(context);
            _cheepService = new CheepService(_cheepRepository, _authorRepository);


            await _cheepService.Follow("1", "12");


            List<AuthorDTO> list = await _cheepService.GetFollowers("Roger Histand");
            Assert.Equal("Adrian", list[0].Name);


            await _cheepService.Unfollow("1", "12");


            List<AuthorDTO> list2 = await _cheepService.GetFollowers("Roger Histand");
            Assert.False(list2.Any());

        }

    }

    [Fact]
    public async void ReadUserAndFollowerMessages()
    {
        using (var scope = _serviceProvider.CreateScope())
        {

            var context = scope.ServiceProvider.GetService<CheepDBContext>();
            _cheepRepository = new CheepRepository(context);
            _authorRepository = new AuthorRepository(context);
            _cheepService = new CheepService(_cheepRepository, _authorRepository);


            await _cheepService.Follow("11", "12");


            List<AuthorDTO> list = await _cheepService.GetFollowers("Helge");

            List<CheepDTO> listCheeps = await _cheepService.ReadUserMessages("Helge", 0);

            List<CheepDTO> listCheeps2 = await _cheepService.ReadUserAndFollowerMessages("Helge", 0);
            Assert.True(1 == list.Count);
            Assert.True(listCheeps.Count < listCheeps2.Count);

        }

    }

    [Fact]
    public async void CountUserAndFollowerMessages()
    {
        using (var scope = _serviceProvider.CreateScope())
        {

            var context = scope.ServiceProvider.GetService<CheepDBContext>();
            _cheepRepository = new CheepRepository(context);
            _authorRepository = new AuthorRepository(context);
            _cheepService = new CheepService(_cheepRepository, _authorRepository);


            await _cheepService.Follow("11", "12");


            List<AuthorDTO> list = await _cheepService.GetFollowers("Helge");

            List<CheepDTO> listCheeps = await _cheepService.ReadUserMessages("Helge", 0);
            int count1 = await _cheepService.CountUserMessages("Helge");
            int count2 = await _cheepService.CountUserMessages("Adrian");
            int count3 = await _cheepService.CountUserAndFollowerMessages("Helge");
            Assert.Equal(count1 + count2, count3);

        }

    }


    [Fact]
    public async void CountPublicMessages()
    {
        using (var scope = _serviceProvider.CreateScope())
        {

            var context = scope.ServiceProvider.GetService<CheepDBContext>();
            _cheepRepository = new CheepRepository(context);
            _authorRepository = new AuthorRepository(context);
            _cheepService = new CheepService(_cheepRepository, _authorRepository);


            int count = await _cheepService.CountPublicMessages();

            // Should be exactly 657
            Assert.Equal(658, count);
        }

    }

    [Fact]
    public async void CountUserMessages()
    {
        using (var scope = _serviceProvider.CreateScope())
        {

            var context = scope.ServiceProvider.GetService<CheepDBContext>();
            _cheepRepository = new CheepRepository(context);
            _authorRepository = new AuthorRepository(context);
            _cheepService = new CheepService(_cheepRepository, _authorRepository);


            int count = await _cheepService.CountUserMessages("Helge");

            // Should be exactly 1
            Assert.Equal(1, count);
        }

    }

    [Fact]
    public async void FindAuthorByName()
    {
        using (var scope = _serviceProvider.CreateScope())
        {

            var context = scope.ServiceProvider.GetService<CheepDBContext>();
            _cheepRepository = new CheepRepository(context);
            _authorRepository = new AuthorRepository(context);
            _cheepService = new CheepService(_cheepRepository, _authorRepository);


            List<AuthorDTO> list = await _cheepService.FindAuthorByName("Hel");
            string authorName = list[0].Name;

            Assert.Equal("Helge", authorName);
        }
    }

    [Fact]
    public async void FindAuthorByEmail()
    {
        using (var scope = _serviceProvider.CreateScope())
        {

            var context = scope.ServiceProvider.GetService<CheepDBContext>();
            _cheepRepository = new CheepRepository(context);
            _authorRepository = new AuthorRepository(context);
            _cheepService = new CheepService(_cheepRepository, _authorRepository);


            List<AuthorDTO> list = await _cheepService.FindAuthorByEmail("ropf@itu");
            string authorName = list[0].Name;

            Assert.Equal("Helge", authorName);
        }
    }

     [Fact]
    public async void FindAuthors()
    {
        using (var scope = _serviceProvider.CreateScope())
        {

            var context = scope.ServiceProvider.GetService<CheepDBContext>();
            _cheepRepository = new CheepRepository(context);
            _authorRepository = new AuthorRepository(context);
            _cheepService = new CheepService(_cheepRepository, _authorRepository);


            List<AuthorDTO> list = await _cheepService.FindAuthors("J");
            string authorName = list[0].Name;

            Assert.Equal("Jacqualine Gilcoine", list[0].Name);
            Assert.Equal("Johnnie Calixto", list[1].Name);
            Assert.Equal("Malcolm Janski", list[2].Name);


        }
    }

     [Fact]
    public async void CreateAuthor()
    {
        using (var scope = _serviceProvider.CreateScope())
        {

            var context = scope.ServiceProvider.GetService<CheepDBContext>();
            _cheepRepository = new CheepRepository(context);
            _authorRepository = new AuthorRepository(context);
            _cheepService = new CheepService(_cheepRepository, _authorRepository);

            AuthorDTO author = new() { Name = "Helge2", Email = "Helg2@mail.dk" };
            await _cheepService.CreateAuthor(author);
            AuthorDTO createdAuthor = await _cheepService.FindSpecificAuthorByName("Helge2");

            Assert.Equal("Helge2", createdAuthor.Name);
        }
    }

    [Fact]
    public async void FindSpecificAuthorByName()
    {
        using (var scope = _serviceProvider.CreateScope())
        {

            var context = scope.ServiceProvider.GetService<CheepDBContext>();
            _cheepRepository = new CheepRepository(context);
            _authorRepository = new AuthorRepository(context);
            _cheepService = new CheepService(_cheepRepository, _authorRepository);

            AuthorDTO author = await _cheepService.FindSpecificAuthorByName("Johnnie Calixto");
            Assert.Equal("Johnnie Calixto", author.Name);
            await Assert.ThrowsAsync<NullReferenceException>(async () => await _cheepService.FindSpecificAuthorByName("J"));

        }
    }

     [Fact]
    public async void FindSpecificAuthorById()
    {
        using (var scope = _serviceProvider.CreateScope())
        {

            var context = scope.ServiceProvider.GetService<CheepDBContext>();
            _cheepRepository = new CheepRepository(context);
            _authorRepository = new AuthorRepository(context);
            _cheepService = new CheepService(_cheepRepository, _authorRepository);

            AuthorDTO author = await _cheepService.FindSpecificAuthorById("1");
            Assert.Equal("Roger Histand", author.Name);

        }
    }

         [Fact]
    public async void FindSpecificAuthorByEmail()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetService<CheepDBContext>();
            _cheepRepository = new CheepRepository(context);
            _authorRepository = new AuthorRepository(context);
            _cheepService = new CheepService(_cheepRepository, _authorRepository);

            AuthorDTO author = await _cheepService.FindSpecificAuthorByEmail("ropf@itu.dk");
            Assert.Equal("Helge", author.Name);
            await Assert.ThrowsAsync<NullReferenceException>(async () => await _cheepService.FindSpecificAuthorByEmail("r"));
        }
    }



    [Fact]
    public async void UpdateMessage()
    {
        using (var scope = _serviceProvider.CreateScope())
        {

            var context = scope.ServiceProvider.GetService<CheepDBContext>();
            _cheepRepository = new CheepRepository(context);
            _authorRepository = new AuthorRepository(context);
            _cheepService = new CheepService(_cheepRepository, _authorRepository);

            bool messageCreated = false;


            CheepDTO newMessage = new() { Author = "Helge", AuthorId = "11", Text = "I love group 13!", Timestamp = 12345, Likes = 0 };
            List<CheepDTO> prevList = await _cheepService.ReadUserMessages("Helge", 0);


            await _cheepService.UpdateMessage(newMessage, 656);
            List<CheepDTO> newList = await _cheepService.ReadUserMessages("Helge", 0);


            foreach (CheepDTO item in newList)
            {
                if (item.Text.Equals(newMessage.Text))
                {
                    messageCreated = true;
                    break;
                }

            }

            Assert.True(newList.Count == prevList.Count);
            Assert.True(messageCreated);
        }
    }

    [Fact]
    public async void AddLike()
    {
        using (var scope = _serviceProvider.CreateScope())
        {

            var context = scope.ServiceProvider.GetService<CheepDBContext>();
            _cheepRepository = new CheepRepository(context);
            _authorRepository = new AuthorRepository(context);
            _cheepService = new CheepService(_cheepRepository, _authorRepository);

            //Likes own cheep (is allowed)
            await _cheepService.AddLike(656, "11");

            //Other likes same cheep
            await _cheepService.AddLike(656, "12");

            List<CheepDTO> cheeps =  await _cheepService.ReadUserMessages("Helge", 0);

            Assert.Equal(2, cheeps[0].Likes);
        }
    }








}