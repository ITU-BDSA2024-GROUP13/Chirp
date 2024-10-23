namespace Chirp.Core;

/// <summary>
/// This is a message data transfer object that is used to transfer message data between the client and the server.
/// </summary>

public class MessageDTO
{
    public string username;

    public int timestamp;

    public string message;

    public string author;

}