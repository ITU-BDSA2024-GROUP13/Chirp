namespace Chirp.Core.DTO;


public class CheepDTO
{
    public required long Timestamp { get; set; }

    public required string Text { get; set; }

    public required string Author { get; set; }

    public required string AuthorId { get; set; }

    public required int Likes { get; set; }


}