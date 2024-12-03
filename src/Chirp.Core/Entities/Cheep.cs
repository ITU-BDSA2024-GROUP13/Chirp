using System.ComponentModel.DataAnnotations;

namespace Chirp.Core.Entities;

/// <summary>
/// Represents a cheep (post) in the Chirp application.
/// A cheep stores content, metadata, and user interactions.
/// </summary>
public class Cheep
{
    /// <summary>
    /// Gets or sets the unique identifier for the cheep.
    /// </summary>
    [Key]
    public int CheepId;

    /// <summary>
    /// Gets or sets the text content of the cheep.
    /// The content is limited to 160 characters.
    /// </summary>
    [Required]
    [StringLength(160)]
    public required string Text { get; set; }

    /// <summary>
    /// Gets or sets the timestamp indicating when the cheep was created.
    /// </summary>
    public required DateTime TimeStamp { get; set; }

    /// <summary>
    /// Gets or sets the ID of the author who created the cheep.
    /// </summary>
    public required string AuthorId { get; set; }

    /// <summary>
    /// Gets or sets the list of authors who liked the cheep.
    /// </summary>
    public required List<Author> Likes { get; set; }

    /// <summary>
    /// Gets or sets the list of authors who disliked the cheep.
    /// </summary>
    public required List<Author> Dislikes { get; set; }

    /// <summary>
    /// Gets or sets the local like ratio for the cheep.
    /// This value is calculated based on likes and dislikes.
    /// </summary>
    public float LocalLikeRatio { get; set; } = 0;

    /// <summary>
    /// Gets or sets the author associated with the cheep.
    /// Represents a navigation property for the author entity.
    /// </summary>
    public Author? Author { get; set; }
}
