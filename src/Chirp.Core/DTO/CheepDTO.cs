namespace Chirp.Core.DTO.CheepDTO;


public class CheepDTO
{
    public required int Id { get; set; }
    public required long Timestamp { get; set; }

    public required string Text { get; set; }

    public required string Author { get; set; }

    public required string AuthorId { get; set; }

    public required int Likes { get; set; }

    public required int Dislikes { get; set; }

    public string? Image { get; set; }
}

public class NewCheepDTO
{
    public required long Timestamp { get; set; }

    public required string Text { get; set; }

    public required string Author { get; set; }

    public required string AuthorId { get; set; }

    public int Likes { get; set; } = 0;

    public int Dislikes { get; set; } = 0;

    public string? Image { get; set; }
}

public class UpdateCheepDTO
{
    public required string Text { get; set; }
}

public class CheepDTOForRelevance
{
    public required int Id { get; set; }
    public required long Timestamp { get; set; }
    public required string Author { get; set; }
    public required int Dislikes { get; set; }
    public required float LocalLikeRatio { get; set; }
    public required bool isFollowing { get; set; }

    public required bool isDisliked { get; set; }

    public string? Image { get; set; }

}