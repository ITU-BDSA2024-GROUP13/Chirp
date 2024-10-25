using System.ComponentModel.DataAnnotations;

namespace Chirp.Repositories;

public class Author
{
    public required int AuthorId { get; set; }

    public required string Name { get; set; }
    public required string Email { get; set; }
    public ICollection<Cheep>? Cheeps { get; set; }



}