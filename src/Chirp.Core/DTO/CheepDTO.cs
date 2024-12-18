namespace Chirp.Core.DTO;


/// <summary>
/// Data transfer object for cheeps. Used when reading cheeps from the database
/// </summary>
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

/// <summary>
/// Data transfer object for new cheeps.
/// </summary>
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

/// <summary>
/// Data transfer object for cheeps, whenever someone wants to edit their text.
/// </summary>
public class UpdateCheepDTO
{
    public required string Text { get; set; }
}

/// <summary>
/// Data transfer object for cheeps, when handling the relevance algorithm
/// </summary>
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