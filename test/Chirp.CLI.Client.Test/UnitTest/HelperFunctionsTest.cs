namespace UnitTest;

public class HelperFunctionsTest {

    [Fact]

    public void TestFromUnixTimeToDateTime(){

        long unixTime = 1610000000;
        DateTime expected = new DateTime(2021, 1, 7, 6, 13, 20, DateTimeKind.Utc).ToLocalTime();

        DateTime actual = HelperFunctions.FromUnixTimeToDateTime(unixTime);

        Assert.Equal(expected, actual);
    }


}
