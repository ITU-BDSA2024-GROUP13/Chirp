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

        [Fact]
    public async void ReadPublicMessages()
    {
        using (var scope = _serviceProvider.CreateScope()){

            var context = scope.ServiceProvider.GetService<CheepDBContext>();
            _cheepRepository = new CheepRepository(context);
            _authorRepository = new AuthorRepository(context);
            _cheepService = new CheepService(_cheepRepository, _authorRepository);


            List<CheepDTO> list = await _cheepService.ReadPublicMessages(0);
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

            [Fact]
    public async void ReadUserMessages()
    {
        using (var scope = _serviceProvider.CreateScope()){

            var context = scope.ServiceProvider.GetService<CheepDBContext>();
            _cheepRepository = new CheepRepository(context);
            _authorRepository = new AuthorRepository(context);
            _cheepService = new CheepService(_cheepRepository, _authorRepository);


            List<CheepDTO> list = await _cheepService.ReadUserMessages("Helge", 0);
            string authorName = list[0].Author;
            Boolean otherAuthor = false;


            foreach (CheepDTO item in list)
            {
                if(!item.Author.Equals(authorName)){
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
        using (var scope = _serviceProvider.CreateScope()){

            var context = scope.ServiceProvider.GetService<CheepDBContext>();
            _cheepRepository = new CheepRepository(context);
            _authorRepository = new AuthorRepository(context);
            _cheepService = new CheepService(_cheepRepository, _authorRepository);

            bool messageCreated = false;

            List<CheepDTO> prevList = await _cheepService.ReadUserMessages("Helge", 0);

            CheepDTO newMessage = new() { Author = "Helge", AuthorId = 11, Text = "I love group 13!", Timestamp = 12345};
            
            await _cheepRepository.CreateMessage(newMessage);

            List<CheepDTO> newList = await _cheepService.ReadUserMessages("Helge", 0);


            foreach (CheepDTO item in newList)
            {
                if(item.Text.Equals(newMessage.Text)){
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
        using (var scope = _serviceProvider.CreateScope()){

            var context = scope.ServiceProvider.GetService<CheepDBContext>();
            _cheepRepository = new CheepRepository(context);
            _authorRepository = new AuthorRepository(context);
            _cheepService = new CheepService(_cheepRepository, _authorRepository);

            bool messageCreated = false;

            List<CheepDTO> prevList = await _cheepService.ReadUserMessages("Helge", 0);

            CheepDTO newMessage = new() { Author = "Helge", AuthorId = 11, Text = "I love group 13! " + 
            "I love group 13! I love group 13! I love group 13! I love group 13! I love group 13! I love group 13! I love group 13! I love group 13! I love !", Timestamp = 12345};
            
            await _cheepService.CreateMessage(newMessage);

            List<CheepDTO> newList = await _cheepService.ReadUserMessages("Helge", 0);


            foreach (CheepDTO item in newList)
            {
                if(item.Text.Equals(newMessage.Text)){
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
        using (var scope = _serviceProvider.CreateScope()){

            var context = scope.ServiceProvider.GetService<CheepDBContext>();
            _cheepRepository = new CheepRepository(context);
            _authorRepository = new AuthorRepository(context);
            _cheepService = new CheepService(_cheepRepository, _authorRepository);

            bool messageCreated = false;

            List<CheepDTO> prevList = await _cheepService.ReadUserMessages("Helge2", 0);

            CheepDTO newMessage = new() { Author = "Helge2", AuthorId = 13, Text = "I love group 13!", Timestamp = 12345};
            
            await _cheepService.CreateMessage(newMessage);

            List<CheepDTO> newList = await _cheepService.ReadUserMessages("Helge2", 0);


            foreach (CheepDTO item in newList)
            {
                if(item.Text.Equals(newMessage.Text)){
                    messageCreated = true;
                    break;
                }
            }

            Assert.True(newList.Count > prevList.Count);
            Assert.True(messageCreated);

        }
     
    }

     [Fact]
    public async void CreateAuthorFromNewMessageAlternative()
    {
        using (var scope = _serviceProvider.CreateScope()){

            var context = scope.ServiceProvider.GetService<CheepDBContext>();
            _cheepRepository = new CheepRepository(context);
            _authorRepository = new AuthorRepository(context);
            _cheepService = new CheepService(_cheepRepository, _authorRepository);

            bool messageCreated = false;

            List<CheepDTO> prevList = await _cheepService.ReadUserMessages("Helg", 0);

            CheepDTO newMessage = new() { Author = "Helg", AuthorId = 13, Text = "I love group 13!", Timestamp = 12345};
            
            await _cheepService.CreateMessage(newMessage);

            List<CheepDTO> newList = await _cheepService.ReadUserMessages("Helg", 0);


            foreach (CheepDTO item in newList)
            {
                if(item.Text.Equals(newMessage.Text)){
                    messageCreated = true;
                    break;
                }
            }

            Assert.True(newList.Count > prevList.Count);
            Assert.True(messageCreated);

        }
     
    }

   

}