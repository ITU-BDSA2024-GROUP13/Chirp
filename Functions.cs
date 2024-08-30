class Functions
{
    private static DateTime UnixTimeStampToDatetime(long UnixTimeStamp)
    {
        return new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(UnixTimeStamp).ToLocalTime();
    }

    public static void ConsoleMessage(string author, String message)
    {
        if (message == null)
            throw new ArgumentException("Please write a message");
        
        DateTime currentTime = DateTime.UtcNow;
        long unixTime = ((DateTimeOffset)currentTime).ToUnixTimeSeconds();
        
        Console.WriteLine(author + " " + message + " " + UnixTimeStampToDatetime(unixTime));
    } 
}
