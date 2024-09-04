//namespace Chirp.CLI;

public record Cheep
{
    public required string userName {get; init; }
    public required string message {get; init; }
    public required int unixTime { get; init; }
}



