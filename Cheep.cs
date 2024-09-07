//namespace Chirp.CLI;

using Chirp.CLI;

public record Cheep
{
    public required string Author {get; init; }
    public required string Message {get; init; }
    public required long Timestamp { get; init; }

    override 
    public string ToString()
    {
        var formattedTimeStamp = HelperFunctions.FromUnixTimeToDateTime(Timestamp);
        return $"{Author} @ {formattedTimeStamp}: {Message}";
    }
}



