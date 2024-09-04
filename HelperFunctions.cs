namespace Chirp.CLI;

public static class HelperFunctions
{
    public static DateTime FromUnixTimeToDateTime(long unixTime)
    {
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTime).ToLocalTime();
        return dateTime;        
    }
}