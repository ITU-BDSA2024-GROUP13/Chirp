using System.ComponentModel.DataAnnotations;

namespace Chirp.Core.Entities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

/// <summary>
/// This is a cheep object which is used to store data about a cheep in a database.
/// </summary>
public class Cheep
{
    [Key]
    public int CheepId;
    public required string Text { get; set; }
    public required DateTime TimeStamp { get; set; }
    public required int AuthorId { get; set; }
    public Author Author { get; set; }
}