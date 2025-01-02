namespace Chirp.Core.DTO.AuthorDTO;

/// <summary>
/// This is a author data transfer object that is used to transfer author data between the client and the server.
/// </summary>
public class AuthorDTO
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }

    public string? PhoneNumber { get; set; }
}

public class NewAuthorDTO
{
    public required string Name { get; set; }
    public required string Email { get; set; }
}