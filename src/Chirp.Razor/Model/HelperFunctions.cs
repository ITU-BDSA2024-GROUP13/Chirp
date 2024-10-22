namespace Chirp.Razor.Model;


/**
 * A list of functions which do not match an ideal responsible class
 */
public static class HelperFunctions
{
    /**
     * <summary>
     * Converting unix time from a long, into a date. <br />
     * <example> 1725801466  ->  9/8/2024 </example>
     * Note that the order is month/day/year.
     * </summary>
     */
    public static DateTime FromUnixTimeToDateTime(long unixTime)
    {

        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddMilliseconds(unixTime).ToLocalTime();

        return dateTime;
    }

}