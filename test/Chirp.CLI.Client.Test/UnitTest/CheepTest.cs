namespace UnitTest;
using Xunit;
using Chirp.CLI.Client;

public class CheepTest
{
    
    [Fact]
    public void TestingCheepFieldAuthorIfNullable()
    {
        string author = null;
        var message = "Hello";
        var timestamp = 123456789;

        Assert.Throws<System.ArgumentException>(() => new Cheep{ Author = author, Message = message, Timestamp = timestamp });
        
    }

    [Fact]
    public void TestingCheepFieldMessageIfNullable()
    {
        string author = "rolf";
        string message = null;
        var timestamp = 123456789;

        Assert.Throws<System.ArgumentException>(() => new Cheep{ Author = author, Message = message, Timestamp = timestamp });
        
    }

    [Fact]
    public void TestingCheepFieldTimeStampLessThanZero()
    {
        string author = "rolf";
        var message = "Hello";
        var timestamp = -1;

        Assert.Throws<System.ArgumentException>(() => new Cheep{ Author = author, Message = message, Timestamp = timestamp });
        
    }
}