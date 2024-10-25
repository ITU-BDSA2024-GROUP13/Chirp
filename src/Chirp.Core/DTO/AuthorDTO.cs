namespace Chirp.Repositories;

/// <summary>
/// This is a author data transfer object that is used to transfer author data between the client and the server.
/// </summary>
public class AuthorDTO
{
    public required int Id { get; set; }
    public required string name { get; set; }
    public required string email { get; set; }

}