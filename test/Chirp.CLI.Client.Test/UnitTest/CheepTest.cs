namespace UnitTest;

public class CheepTest
{
    
    [Fact]
    public void TestingCheepFieldsIfNullable()
    {
        var cheep = new Chirp.CLI.Client.Cheep{Author = null, Message = null, Timestamp = -1 };
        Assert.True(cheep.Author != null);
        Assert.True(cheep.Message == null);
        Assert.True(cheep.Timestamp == -1);
    }
}