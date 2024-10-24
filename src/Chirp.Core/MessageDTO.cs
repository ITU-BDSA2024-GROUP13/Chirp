namespace Chirp.Core;

/// <summary>
/// This is a message data transfer object that is used to transfer message data between the client and the server.
/// </summary>

public class MessageDTO
{
    public required string username;

    public required int timestamp;

    public required string message;

    public required string author;

}