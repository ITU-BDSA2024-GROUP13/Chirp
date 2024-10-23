using System.ComponentModel.DataAnnotations;

namespace Chirp.Repositories;

public class Author
{
    [Key]
    public int AuthorId {get ; set; }
    public string Name { get; set; }
    public string Email { get; set; }    
    public ICollection<Cheep> Cheeps {get ; set;}

    public Author(int authorId, string name, string email, ICollection<Cheep> cheeps)
    {
        this.AuthorId = authorId;
        this.Name = name;
        this.Email = email;
        this.Cheeps = cheeps;
    }

    public Author()
    {

    }

    public string GetName()
    {
        return Name;
    }

    public string GetEmail()
    {
        return Email;
    }

    public ICollection<Cheep> GetCheeps()
    {
        return Cheeps;
    }
}

