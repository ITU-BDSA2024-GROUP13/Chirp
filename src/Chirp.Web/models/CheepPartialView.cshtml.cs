namespace Chirp.Web.models;

/// <summary>
/// Represents a partial view of a Cheep (a tweet-like post).
/// </summary>
public class CheepPartialView
{
    /// <summary>
    /// Gets or sets the unique identifier for the Cheep.
    /// </summary>
    public required int? Id { get; set; }

    /// <summary>
    /// Gets or sets the author of the Cheep.
    /// </summary>
    public required string Author { get; set; }

    /// <summary>
    /// Gets or sets the body/content of the Cheep.
    /// </summary>
    public required string Body { get; set; }

    /// <summary>
    /// Gets or sets whether the Cheep has been liked.
    /// </summary>
    public required bool IsLiked { get; set; }

    /// <summary>
    /// Gets or sets whether the Cheep has been disliked.
    /// </summary>
    public required bool IsDisliked { get; set; }

    /// <summary>
    /// Gets or sets the Unix timestamp representing the creation date of the Cheep.
    /// </summary>
    public required long Date { get; set; }

    /// <summary>
    /// Gets or sets the number of likes the Cheep has received.
    /// </summary>
    public required int Likes { get; set; }

    /// <summary>
    /// Gets or sets the number of dislikes the Cheep has received.
    /// </summary>
    public required int Dislikes { get; set; }

    /// <summary>
    /// Generates the HTML ID for the like button based on the Cheep's ID.
    /// </summary>
    /// <returns>A string representing the ID of the like button.</returns>
    public string LikeButtonId()
    {
        return Id + "like";
    }

    /// <summary>
    /// Generates the HTML ID for the dislike button based on the Cheep's ID.
    /// </summary>
    /// <returns>A string representing the ID of the dislike button.</returns>
    public string DislikeButtonId()
    {
        return Id + "dislike";
    }

    /// <summary>
    /// Converts a Unix timestamp to a DateTime object.
    /// </summary>
    /// <param name="value">The Unix timestamp to convert.</param>
    /// <returns>A DateTime representation of the Unix timestamp.</returns>
    public DateTime ToDateTime(long value)
    {
        return Repositories.HelperFunctions.FromUnixTimeToDateTime(value);
    }
}
