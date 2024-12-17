using Chirp.Infrastructure.Repositories;

namespace Chirp.Test.Infrastructure.Repositories;

public class HelperFunctionsTest
{
    [Fact]
    public void UnixTimeConversion()
    {
        long unix = 1730213366;

        DateTime dateTime = HelperFunctions.FromUnixTimeToDateTime(unix);

        Assert.Equal(unix, HelperFunctions.FromDateTimetoUnixTime(dateTime));
    }

    [Fact]
    public void danishFormat()
    {
        long unix = 1730213300000;

        DateTime dateTime = HelperFunctions.FromUnixTimeToDateTime(unix);
        string danishFormat = HelperFunctions.ReformatDateTimetoDanishFormat(dateTime);

        Assert.Equal("29.10.2024 15.48.20", danishFormat);
    }



}