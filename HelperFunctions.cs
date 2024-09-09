using System.Text.RegularExpressions;

namespace Chirp.CLI;

public static class HelperFunctions
{
    public static DateTime FromUnixTimeToDateTime(long unixTime)
    {
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTime).ToLocalTime();
        return dateTime;        
    }
    
    public static string formatFromFileToConsole(string line)
    {
        var regex = new Regex(
            "^(?<author>[æøåa-zÆØÅA-z0-9_-]*),[\"\"](?<message>.*)[\"\"],(?<timeStamp>[0-9]*)$");
        var match = regex.Match(line);
        var author = match.Groups["author"];
        var message = match.Groups["message"];
        var timestamp = HelperFunctions.FromUnixTimeToDateTime(int.Parse(match.Groups["timeStamp"].Value));
    
        return String.Format("{0} @ {1}: {2}", author, timestamp, message);
    }
}