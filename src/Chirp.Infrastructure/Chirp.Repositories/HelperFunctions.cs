using System.Globalization;

namespace Chirp.Repositories;


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
     * <returns> DateTime </returns>
     */
    public static DateTime FromUnixTimeToDateTime(long unixTime)
    {
        DateTime dateTime = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddMilliseconds(unixTime).ToLocalTime();
        return dateTime;
    }

    public static string ReformatDateTimetoDanishFormat(DateTime dateTime)
    {
        CultureInfo danish = new("da");
        String danishFormat = dateTime.ToString("G", danish);
        return danishFormat;
    }

    public static long FromDateTimetoUnixTime(DateTime dateTime)
    {
        long unixTime = ((DateTimeOffset)dateTime).ToUnixTimeMilliseconds();

        return unixTime;

    }

}