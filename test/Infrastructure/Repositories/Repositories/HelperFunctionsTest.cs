using Chirp.Repositories;

namespace Repositories;

public class HelperFunctionsTest
{
    [Fact]
    public void Test1()
    {
        long unix = 1730213366;

        DateTime dateTime = HelperFunctions.FromUnixTimeToDateTime(unix);

        Assert.Equals(unix, DateTime.UtcNow dateTime.UtcNo)


    }
}