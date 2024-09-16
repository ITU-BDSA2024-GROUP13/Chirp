using CsvHelper;

namespace Chirp.CLI.Client;

/**
 * <summary>
 * The <c>Cheep</c> record allows for messages to be stored
 * into a csv file.
 * </summary>
 */
public record Cheep
{
    public required string Author {get; init; }
    public required string Message {get; init; }
    public required long Timestamp { get; init; }


    public void Validate()
    {
        if (string.IsNullOrEmpty(Author)){
            throw new ArgumentException("Author cannot be null or empty");
        }
        if (string.IsNullOrEmpty(Message)){
            throw new ArgumentException("Message cannot be null or empty");
        }
        if (Timestamp < 0){
            throw new ArgumentException("Timestamp cannot be less than 0");
        }
    }

    /**
     * <example> andrebirk @ 9/8/2024 3:17:46 PM: Hellooo <br/>
     * <c>Author</c> = andrebirk <br/>
     * <c>Message</c> = Hellooo <br/>
     * <c>Timestamp</c> = 1725801466
     * </example>
     */
    override 
    public string ToString()
    {
        var formattedTimeStamp = HelperFunctions.FromUnixTimeToDateTime(Timestamp);
        return $"{Author} @ {formattedTimeStamp}: {Message}";
    }
}



