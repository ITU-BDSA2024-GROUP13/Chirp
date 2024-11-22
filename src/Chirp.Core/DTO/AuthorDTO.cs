namespace Chirp.Core.DTO;

/// <summary>
/// This is a author data transfer object that is used to transfer author data between the client and the server.
/// </summary>
public class AuthorDTO
{
    public string Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }

    public int count { get; set; }

}