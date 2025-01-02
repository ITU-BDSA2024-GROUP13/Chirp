using Chirp.Core.DTO.CheepDTO;
using Chirp.Infrastructure.Repositories;
using Humanizer;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Sdk;

namespace Chirp.Test.Infrastructure.Repositories;

public class CheepRepositoryTest : IDisposable
{
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Dereference of a possibly null reference.

    private ServiceProvider _serviceProvider;



    public CheepRepositoryTest()
    {

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
    public async void ReadPublicMessages()
    {
        using (var scope = _serviceProvider.CreateScope())
        {

            using (var context = scope.ServiceProvider.GetService<CheepDBContext>())
            {
                var repo = new CheepRepository(context);

                List<CheepDTO> list = await repo.ReadPublicMessages(32, 0);
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
    }

    [Fact]
    public async void ReadPublicMessagesbyOldest()
    {
        using (var scope = _serviceProvider.CreateScope())
        {

            using (var context = scope.ServiceProvider.GetService<CheepDBContext>())
            {
                var repo = new CheepRepository(context);

                List<CheepDTO> list = await repo.ReadPublicMessagesbyOldest(32, 0);

                // Should not be larger than the take value
                Assert.False(list.Count > 32);
                // The most recent message in the test db
                Assert.Equal("Hello, BDSA students!", list[0].Text);
            }
        }
    }

    [Fact]
    public async void ReadPublicMessagesbyMostLiked()
    {
        using (var scope = _serviceProvider.CreateScope())
        {

            using (var context = scope.ServiceProvider.GetService<CheepDBContext>())
            {
                var repo = new CheepRepository(context);

                await repo.AddLike(1, "11");
                await repo.AddLike(1, "10");
                await repo.AddLike(1, "9");
                await repo.AddLike(1, "8");
                await repo.AddLike(1, "7");
                await repo.AddLike(1, "6");
                await repo.AddLike(1, "5");

                await repo.AddLike(2, "11");
                await repo.AddLike(2, "10");
                await repo.AddLike(2, "9");
                await repo.AddLike(2, "8");
                await repo.AddLike(2, "7");
                await repo.AddLike(2, "6");




                List<CheepDTO> list = await repo.ReadPublicMessagesbyMostLiked(32, 0);
                // Should not be larger than the take value
                Assert.False(list.Count > 32);
                // The most recent message in the test db
                Assert.Equal("They were married in Chicago, with old Smith, and was expected aboard every day; meantime, the two went past me.", list[0].Text);
                Assert.Equal("And then, as he listened to all that's left o' twenty-one people.", list[1].Text);

            }
        }
    }

    [Fact]
    public async void ReadPublicMessagesbyRelevance()
    {
        using (var scope = _serviceProvider.CreateScope())
        {

            using (var context = scope.ServiceProvider.GetService<CheepDBContext>())
            {
                var repo = new CheepRepository(context);

                //659
                await repo.CreateMessage(new NewCheepDTO
                {
                    Author = "Jacqualine Gilcoine",
                    AuthorId = "10",
                    Text = "Hello",
                    Timestamp = HelperFunctions.FromDateTimetoUnixTime(DateTime.UtcNow.AddDays(1))
                });

                //660
                await repo.CreateMessage(new NewCheepDTO
                {
                    Author = "Helge",
                    AuthorId = "11",
                    Text = "Hello",
                    Timestamp = HelperFunctions.FromDateTimetoUnixTime(DateTime.UtcNow.AddHours(23))
                });

                //661
                await repo.CreateMessage(new NewCheepDTO
                {
                    Author = "Adrian",
                    AuthorId = "12",
                    Text = "Hello",
                    Timestamp = HelperFunctions.FromDateTimetoUnixTime(DateTime.UtcNow.AddHours(23))
                });

                //662
                await repo.CreateMessage(new NewCheepDTO
                {
                    Author = "Johnnie Calixto",
                    AuthorId = "9",
                    Text = "Hello",
                    Timestamp = HelperFunctions.FromDateTimetoUnixTime(DateTime.UtcNow)
                });



                await repo.AddLike(661, "10");
                await repo.AddLike(661, "11");
                await repo.AddLike(661, "9");
                await repo.AddLike(661, "8");
                await repo.AddLike(661, "7");

                var cheep = await repo.FindSpecificCheepbyId(659);
                var cheep2 = await repo.FindSpecificCheepbyId(660);
                var cheep3 = await repo.FindSpecificCheepbyId(661);
                var cheep5 = await repo.FindSpecificCheepbyId(662);

                Assert.Equal(-25, (DateTime.UtcNow - HelperFunctions.FromUnixTimeToDateTime(cheep.Timestamp)).TotalHours, 0.5);
                Assert.Equal(-24, (DateTime.UtcNow - HelperFunctions.FromUnixTimeToDateTime(cheep2.Timestamp)).TotalHours, 0.5);

                var cheepLocalLikeRatio3 = (float)Math.Log((float)cheep3.Likes, 5);


                Assert.Equal(25, 0 - (DateTime.UtcNow - HelperFunctions.FromUnixTimeToDateTime(cheep.Timestamp)).TotalHours, 0.5);
                Assert.Equal(24, 0 - (DateTime.UtcNow - HelperFunctions.FromUnixTimeToDateTime(cheep2.Timestamp)).TotalHours, 0.5);
                // cheep 661 has gotten 2 more relevance from likes
                Assert.Equal(25, cheepLocalLikeRatio3 - (DateTime.UtcNow - HelperFunctions.FromUnixTimeToDateTime(cheep3.Timestamp)).TotalHours, 0.5);

                List<CheepDTO> list = await repo.ReadPublicMessagesbyRelevance(32, 0, "Helge");
                // Should not be larger than the take value
                Assert.False(list.Count > 32);
                // The most relevant
                Assert.Equal("12", list[0].AuthorId);
                Assert.Equal("10", list[1].AuthorId);
                Assert.Equal("11", list[2].AuthorId);

                await repo.RemoveLike(661, "10");
                await repo.RemoveLike(661, "11");
                await repo.RemoveLike(661, "9");
                await repo.RemoveLike(661, "8");

                await repo.AddLike(660, "11");
                await repo.AddLike(660, "10");

                List<CheepDTO> list2 = await repo.ReadPublicMessagesbyRelevance(32, 0, "Helge");

                Assert.Equal("10", list2[0].AuthorId);
                Assert.Equal("11", list2[1].AuthorId);

                await repo.AddDisLike(659, "11");
                List<CheepDTO> list4 = await repo.ReadPublicMessagesbyRelevance(32, 0, "Helge");
                // Helge message gets more relevance for Adrian
                Assert.Equal("11", list4[0].AuthorId);

                var authorRepo = new AuthorRepository(context);

                // Adrian follows Helge
                await authorRepo.AddFollower("12", "11");

                List<CheepDTO> list3 = await repo.ReadPublicMessagesbyRelevance(32, 0, "Adrian");
                // Helge message gets more relevance for Adrian
                Assert.Equal("11", list3[0].AuthorId);
                Assert.Equal("10", list3[1].AuthorId);




            }
        }
    }




    [Fact]
    public async void ReadUserMessages()
    {
        using (var scope = _serviceProvider.CreateScope())
        {

            using (var context = scope.ServiceProvider.GetService<CheepDBContext>())
            {
                var repo = new CheepRepository(context);

                List<CheepDTO> list = await repo.ReadUserMessages("Helge", 32, 0);
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
                // The most recent message in the test db
                Assert.False(otherAuthor);
                // The authors timeline should not be empty
                Assert.True(list.Count > 0);
                Assert.Equal("Helge", authorName);
            }
        }
    }





    [Fact]
    public async void CreateMessage()
    {
        using (var scope = _serviceProvider.CreateScope())
        {

            using (var context = scope.ServiceProvider.GetService<CheepDBContext>())
            {
                var repo = new CheepRepository(context);
                bool messageCreated = false;


                NewCheepDTO newMessage = new()
                {
                    Author = "Helge",
                    AuthorId = "11",
                    Text = "I love group 13!",
                    Timestamp = 12345
                };
                List<CheepDTO> prevList = await repo.ReadUserMessages("Helge", 32, 0);


                await repo.CreateMessage(newMessage);
                List<CheepDTO> newList = await repo.ReadUserMessages("Helge", 32, 0);


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
    }

    [Fact]
    public async void UpdateMessage()
    {
        using (var scope = _serviceProvider.CreateScope())
        {

            using (var context = scope.ServiceProvider.GetService<CheepDBContext>())
            {
                var repo = new CheepRepository(context);
                bool messageCreated = false;


                UpdateCheepDTO newMessage = new() { Text = "I love group 13!" };
                List<CheepDTO> prevList = await repo.ReadUserMessages("Helge", 32, 0);


                await repo.UpdateMessage(newMessage, 656);
                List<CheepDTO> newList = await repo.ReadUserMessages("Helge", 32, 0);


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
    }

    [Fact]
    public async void RemoveAllMessages()
    {
        using (var scope = _serviceProvider.CreateScope())
        {

            using (var context = scope.ServiceProvider.GetService<CheepDBContext>())
            {
                var repo = new CheepRepository(context);

                List<CheepDTO> cheeps = await repo.ReadUserMessages("Roger Histand", 300, 0);

                await repo.RemoveCheepsFromUser("Roger Histand");

                List<CheepDTO> nocheeps = await repo.ReadUserMessages("Roger Histand", 300, 0);

                Assert.NotEmpty(cheeps);
                Assert.Empty(nocheeps);
            }
        }
    }

    [Fact]
    public async void AddLike()
    {
        using (var scope = _serviceProvider.CreateScope())
        {

            using (var context = scope.ServiceProvider.GetService<CheepDBContext>())
            {
                var repo = new CheepRepository(context);

                await repo.AddLike(1, "12");

                CheepDTO cheep = await repo.FindSpecificCheepbyId(1);

                Assert.Equal(1, cheep.Likes);

                await repo.AddLike(1, "11");
                await repo.AddLike(1, "11");
                await repo.AddLike(1, "11");
                await repo.AddLike(1, "11");
                await repo.AddLike(1, "12");
                await repo.AddLike(1, "10");

                CheepDTO cheep2 = await repo.FindSpecificCheepbyId(1);

                Assert.Equal(3, cheep2.Likes);
            }
        }
    }
    [Fact]
    public async void RemoveLike()
    {
        using (var scope = _serviceProvider.CreateScope())
        {

            using (var context = scope.ServiceProvider.GetService<CheepDBContext>())
            {
                var repo = new CheepRepository(context);

                await repo.AddLike(1, "12");

                CheepDTO cheep = await repo.FindSpecificCheepbyId(1);

                Assert.Equal(1, cheep.Likes);

                await repo.RemoveLike(1, "12");

                CheepDTO cheep2 = await repo.FindSpecificCheepbyId(1);

                Assert.Equal(0, cheep2.Likes);
            }
        }
    }


    [Fact]
    public async void RemoveAllLikes()
    {
        using (var scope = _serviceProvider.CreateScope())
        {

            using (var context = scope.ServiceProvider.GetService<CheepDBContext>())
            {
                var repo = new CheepRepository(context);

                await repo.AddLike(1, "12");

                CheepDTO cheep = await repo.FindSpecificCheepbyId(1);

                Assert.Equal(1, cheep.Likes);

                await repo.AddLike(1, "11");
                await repo.AddLike(1, "10");
                await repo.AddLike(1, "9");
                await repo.AddLike(1, "8");
                await repo.AddLike(1, "7");
                await repo.AddLike(1, "6");

                CheepDTO cheep2 = await repo.FindSpecificCheepbyId(1);

                Assert.Equal(7, cheep2.Likes);

                await repo.RemoveAllLikes(1);

                CheepDTO cheep3 = await repo.FindSpecificCheepbyId(1);

                Assert.Equal(0, cheep3.Likes);

            }
        }
    }

    [Fact]
    public async void RemoveDislike()
    {
        using (var scope = _serviceProvider.CreateScope())
        {

            using (var context = scope.ServiceProvider.GetService<CheepDBContext>())
            {
                var repo = new CheepRepository(context);

                await repo.AddDisLike(1, "12");

                CheepDTO cheep = await repo.FindSpecificCheepbyId(1);

                Assert.Equal(1, cheep.Dislikes);

                await repo.RemoveDislike(1, "12");
                await repo.RemoveDislike(1, "12");


                CheepDTO cheep2 = await repo.FindSpecificCheepbyId(1);

                Assert.Equal(0, cheep2.Dislikes);
            }
        }
    }

    [Fact]
    public async void AddDislike()
    {
        using (var scope = _serviceProvider.CreateScope())
        {

            using (var context = scope.ServiceProvider.GetService<CheepDBContext>())
            {
                var repo = new CheepRepository(context);

                await repo.AddDisLike(1, "12");

                CheepDTO cheep = await repo.FindSpecificCheepbyId(1);

                Assert.Equal(1, cheep.Dislikes);

                await repo.AddDisLike(1, "11");
                await repo.AddDisLike(1, "11");
                await repo.AddDisLike(1, "11");
                await repo.AddDisLike(1, "11");
                await repo.AddDisLike(1, "12");
                await repo.AddDisLike(1, "10");

                CheepDTO cheep2 = await repo.FindSpecificCheepbyId(1);

                Assert.Equal(3, cheep2.Dislikes);
            }
        }
    }

    [Fact]
    public async void RemoveAllDislikes()
    {
        using (var scope = _serviceProvider.CreateScope())
        {

            using (var context = scope.ServiceProvider.GetService<CheepDBContext>())
            {
                var repo = new CheepRepository(context);

                await repo.AddDisLike(1, "12");

                CheepDTO cheep = await repo.FindSpecificCheepbyId(1);

                Assert.Equal(1, cheep.Dislikes);

                await repo.AddDisLike(1, "11");
                await repo.AddDisLike(1, "10");
                await repo.AddDisLike(1, "9");
                await repo.AddDisLike(1, "8");
                await repo.AddDisLike(1, "7");
                await repo.AddDisLike(1, "6");

                CheepDTO cheep2 = await repo.FindSpecificCheepbyId(1);

                Assert.Equal(7, cheep2.Dislikes);

                await repo.RemoveAllDislikes(1);

                CheepDTO cheep3 = await repo.FindSpecificCheepbyId(1);

                Assert.Equal(0, cheep3.Dislikes);

            }
        }
    }








}