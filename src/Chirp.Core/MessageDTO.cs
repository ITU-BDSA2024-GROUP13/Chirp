namespace Chirp.Core;

/// <summary>
/// Represents a data transfer object (DTO) for messages in the Chirp application.
/// This class is used to transfer message data between the client and the server.
/// </summary>
public class MessageDTO
{
    /// <summary>
    /// Gets or sets the username of the user associated with the message.
    /// </summary>
    public required string username;

    /// <summary>
    /// Gets or sets the timestamp for the message.
    /// The timestamp is typically represented as a Unix epoch time (seconds since 1970-01-01T00:00:00Z).
    /// </summary>
    public required int timestamp;

    /// <summary>
    /// Gets or sets the content of the message.
    /// </summary>
    public required string message;

    /// <summary>
    /// Gets or sets the author of the message.
    /// This may differ from the username in certain contexts (e.g., for aliases or display names).
    /// </summary>
    public required string author;
}
