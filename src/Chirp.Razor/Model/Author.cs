namespace Chirp.Razor.Model;

public class Author
{
    private string _name { get; set; }
    private string _email { get; set; }
    
    private List<Cheep> _cheeps {get ; set;}

    public Author(string name, string email)
    {
        _name = name;
        _email = email;
    }

    public string GetName()
    {
        return _name;
    }

    public string GetEmail()
    {
        return _email;
    }

    public List<Cheep> GetCheeps()
    {
        return _cheeps;
    }
}

