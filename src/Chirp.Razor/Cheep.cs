namespace Chirp.Razor;

public class Cheep
{
    private string _text { get; set; }
    private DateTime _dateTime { get; set; }
    
    private Author _author { get; set; }
    
    public Cheep(string text, DateTime dateTime)
    {
        _text = text;
        _dateTime = dateTime;
    }

    public string GetText()
    {
        return _text;
    }

    public DateTime GetDateTime()
    {
        return _dateTime;
    }

    public Author GertAuthor()
    {
        return _author;
    }
}