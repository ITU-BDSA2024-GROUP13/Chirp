using Microsoft.AspNetCore.Identity;

namespace Chirp.Core.Entities;

/// <summary>
/// Represents an author or user in the Chirp application.
/// Inherits from <see cref="IdentityUser"/> to provide identity and authentication functionality.
/// </summary>
public class Author : IdentityUser
{
    /// <summary>
    /// Gets or sets the collection of cheeps (posts) created by the author.
    /// </summary>
    public required ICollection<Cheep> Cheeps { get; set; }

    /// <summary>
    /// Gets or sets the collection of authors that this author is following.
    /// </summary>
    public required ICollection<Author> Followers { get; set; }

    /// <summary>
    /// Gets or sets the collection of authors that are following this author.
    /// </summary>
    public required ICollection<Author> FollowedBy { get; set; }

    /// <summary>
    /// Gets or sets the collection of cheeps (posts) that this author has liked.
    /// </summary>
    public required ICollection<Cheep> LikedCheeps { get; set; }

    /// <summary>
    /// Gets or sets the collection of cheeps (posts) that this author has disliked.
    /// </summary>
    public required ICollection<Cheep> DislikedCheeps { get; set; }
}
