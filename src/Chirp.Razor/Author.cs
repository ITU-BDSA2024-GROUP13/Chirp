namespace Chirp.Razor;

public class Author
{
    private string _name { get; set; }
    private string _email { get; set; }

    public Author(string name, string email)
    {
        _name = name;
        _email = email;
    }
}