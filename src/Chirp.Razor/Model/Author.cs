namespace Chirp.Razor.Model;

public class Author
{
    public int Id {get ; set; }
    public string name { get; set; }
    public string email { get; set; }    
    private ICollection<Cheep> _cheeps {get ; set;}

    public Author(string name, string email)
    {
        this.name = name;
        this.email = email;
    }

    public string GetName()
    {
        return name;
    }

    public string GetEmail()
    {
        return email;
    }

    public List<Cheep> GetCheeps()
    {
        return _cheeps;
    }
}

