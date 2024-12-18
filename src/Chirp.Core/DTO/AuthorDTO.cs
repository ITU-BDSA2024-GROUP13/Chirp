namespace Chirp.Core.DTO;

/// <summary>
/// This is an author data transfer object that is used to transfer author data between the client and the server,
/// used when reading from the database.
/// </summary>
public class AuthorDTO
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
}

/// <summary>
/// Author data transfer object, which is used for creating new authors
/// </summary>
public class NewAuthorDTO
{
    public required string Name { get; set; }
    public required string Email { get; set; }
}