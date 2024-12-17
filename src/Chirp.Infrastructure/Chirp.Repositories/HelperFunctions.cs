using System.Globalization;

namespace Chirp.Infrastructure.Repositories;


/// <summary>
/// A collection of helper functions that do not belong to a more specific class.
/// </summary>
public static class HelperFunctions
{
    /// <summary>
    /// Converts a Unix timestamp to a <see cref="DateTime"/> object.
    /// </summary>
    /// <param name="unixTime">The Unix timestamp to convert.</param>
    /// <returns>A <see cref="DateTime"/> object representing the given Unix timestamp.</returns>
    /// <example>
    /// <code>
    /// long unixTime = 1725801466;
    /// DateTime dateTime = HelperFunctions.FromUnixTimeToDateTime(unixTime);
    /// Console.WriteLine(dateTime);  // Output: 9/8/2024
    /// </code>
    /// </example>
    public static DateTime FromUnixTimeToDateTime(long unixTime)
    {
        DateTime dateTime = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        return dateTime.AddMilliseconds(unixTime).ToLocalTime();
    }

    /// <summary>
    /// Reformats the given <see cref="DateTime"/> object to Danish format (month/day/year).
    /// </summary>
    /// <param name="dateTime">The <see cref="DateTime"/> object to format.</param>
    /// <returns>A string representing the <see cref="DateTime"/> in Danish format.</returns>
    public static string ReformatDateTimetoDanishFormat(DateTime dateTime)
    {
        return dateTime.ToString("G", new CultureInfo("da"));
    }

    /// <summary>
    /// Converts a <see cref="DateTime"/> object to a Unix timestamp (milliseconds since 1970-01-01).
    /// </summary>
    /// <param name="dateTime">The <see cref="DateTime"/> object to convert.</param>
    /// <returns>The Unix timestamp as a long value.</returns>
    public static long FromDateTimetoUnixTime(DateTime dateTime)
    {
        long unixTime = ((DateTimeOffset)dateTime).ToUnixTimeMilliseconds();
        return unixTime;
    }

}