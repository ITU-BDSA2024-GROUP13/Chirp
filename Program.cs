DateTime currentTime = DateTime.UtcNow;
long unixTime = ((DateTimeOffset)currentTime).ToUnixTimeSeconds();
// is a method that converts the current time to Unix time.
// Unix time is the number of seconds that have elapsed since 00:00:00 Coordinated Universal Time (UTC),
// Thursday, 1 January 1970.

Console.WriteLine("Unix time: " + unixTime);
unixTime
Console.WriteLine("Current time: " + currentTime);

// Console.WriteLine("User input: " + userInput);

