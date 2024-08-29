DateTime currentTime = DateTime.UtcNow;
long unixTime = ((DateTimeOffset)currentTime).ToUnixTimeSeconds();

Console.WriteLine(unixTime);

DateTimeOffset.UtcNow.ToUnixTimeSeconds();

DateTimeOffset.UtcNow.
