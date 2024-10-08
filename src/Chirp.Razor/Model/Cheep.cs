namespace Chirp.Razor.Model;

public class Cheep
{
    public int Id {get ; set; }
    public string text { get; set; }
    public DateTime dateTime { get; set; }
    public int authorId {get; set; }

    private Author _author { get; set; }
    
    public Cheep(string text, DateTime dateTime)
    {
        this.text = text;
        this.dateTime = dateTime;
    }

    public string GetText()
    {
        return text;
    }

    public DateTime GetDateTime()
    {
        return dateTime;
    }

    public Author GertAuthor()
    {
        return _author;
    }
}