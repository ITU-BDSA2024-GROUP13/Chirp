using Chirp.Core.Entities;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Xunit.Sdk;

namespace Core;

public class AuthorTest
{
    [Fact]
    public void Requirements()
    {
        Author a = new() { Name = "Helge", AuthorId = "1", Email = "Helge@gmail.com", 
        Cheeps = new List<Cheep>(), FollowedBy = new List<Author>(), Followers = new List<Author>() };
        Assert.Empty(a.Cheeps);
    }
}