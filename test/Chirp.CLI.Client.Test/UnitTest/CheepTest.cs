using System.ComponentModel.Design;
using System.Net;

namespace UnitTest;

public class CheepTest
{


    [Fact]
    public void TestingCheepFieldAuthorIfNullable()
    {
        string author = null;
        var message = "Hello";
        var timestamp = 123456789;


        var cheep = new Cheep{Id =0, Author = author, Message = message, Timestamp = timestamp };


        Assert.Throws<System.ArgumentException>(() => cheep.Validate());

    }

    [Fact]
    public void TestingCheepFieldMessageIfNullable()
    {
        string author = "rolf";
        string message = null;
        var timestamp = 123456789;

        var cheep = new Cheep{Id = 0, Author = author, Message = message, Timestamp = timestamp };


        Assert.Throws<System.ArgumentException>(() => cheep.Validate());

    }

    [Fact]
    public void TestingCheepFieldTimeStampLessThanZero()
    {
        string author = "rolf";
        var message = "Hello";
        var timestamp = -1;

        var cheep = new Cheep{Id = 0, Author = author, Message = message, Timestamp = timestamp };


        Assert.Throws<System.ArgumentException>(() => cheep.Validate());

    }

    [Theory]

    [InlineData("rolf", "Hello", 123456789)]
    [InlineData("polu", "goodbye", 9876543321)]

    public void TestingCheepFieldCreation(string author, string message, long timestamp)
    {

        var cheep = new Cheep{Id = 0, Author = author, Message = message, Timestamp = timestamp };

        cheep.Validate();
        Assert.True(cheep.Author == author);
        Assert.True(cheep.Message == message);
        Assert.True(cheep.Timestamp == timestamp);
    }

    [Fact]
    public void TestingCheepToString()
    {

        var author = "polu";
        var message = "Hello world!";
        var timestamp = 1726093906;

        var cheep = new Cheep(){Id = 0, Author = author, Message = message, Timestamp = timestamp };


        Assert.True(cheep.ToString() == "polu @ 9/12/2024 12:31:46AM: Hello world!");
    }
}