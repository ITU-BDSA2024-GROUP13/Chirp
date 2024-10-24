using System.ComponentModel.DataAnnotations;

namespace Chirp.Repositories;

/// <summary>
/// This is a cheep object which is used to store data about a cheep in a database.
/// </summary>

public class Cheep
{
    [Key]
    public required int CheepId;
    public required string Text { get; set; }
    public required DateTime TimeStamp { get; set; }
    public required int AuthorId { get; set; }
    public Author? Author { get; set; }
}