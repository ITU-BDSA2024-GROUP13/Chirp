namespace Chirp.Repositories;

public class CheepDTO
{
    public long Timestamp { get; set; }

    public required string Text { get; set; }

    public required string Author { get; set; }

    public required int AuthorId { get; set; }

}