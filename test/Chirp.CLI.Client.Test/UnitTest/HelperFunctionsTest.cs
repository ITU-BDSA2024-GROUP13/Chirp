namespace UnitTest;

public class HelperFunctionsTest {

    [Fact]
    public void TestFromUnixTimeToDateTimeWithPositiveTime(){

        long unixTime = 1610000000;
        DateTime expected = new DateTime(2021, 1, 7, 6, 13, 20, DateTimeKind.Utc).ToLocalTime();

        DateTime actual = HelperFunctions.FromUnixTimeToDateTime(unixTime);

        Assert.Equal(expected, actual);
    }


    [Fact]
    public void TestFromUnixTimeToDateTimeWithNegativeTime(){

        long unixTime = -86400; 
    
        DateTime expected = new DateTime(1969, 12, 31, 0, 0, 0, DateTimeKind.Utc).ToLocalTime();

        DateTime actual = HelperFunctions.FromUnixTimeToDateTime(unixTime);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestFromUnixTimeToDateTimeWithTooLargeValues(){
        
        long unixTime = long.MaxValue;

        Assert.Throws<System.ArgumentOutOfRangeException>(() => HelperFunctions.FromUnixTimeToDateTime(unixTime));
    }


}
