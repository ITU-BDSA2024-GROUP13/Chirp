using Chirp.Repositories;

namespace Repositories;

public class HelperFunctionsTest
{
    [Fact]
    public void UnixTimeConversion()
    {
        long unix = 1730213366;

        DateTime dateTime = HelperFunctions.FromUnixTimeToDateTime(unix);

        Assert.Equal(unix, HelperFunctions.FromDateTimetoUnixTime(dateTime));
    }
}