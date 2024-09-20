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
    public required int Id {get; set; }

    public required string Author {get; set; }
    public required string Message {get; set; }
    public required long Timestamp { get; set; }

    /**
        * <summary>
        * Validates the Cheep object, ensuring that the Author and  Message are not null or empty
        * and that the Timestamp is not less than 0.
        * </summary>
    */

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



