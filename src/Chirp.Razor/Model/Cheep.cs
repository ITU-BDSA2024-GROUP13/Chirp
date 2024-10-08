namespace Chirp.Razor.Model;

public class Cheep
{
    public int Id {get ; set; }
    public string text { get; set; }
    public long dateTime { get; set; }
    public int authorId {get; set; }

    private Author _author { get; set; }
    
    public Cheep(string text, long dateTime)
    {
        this.text = text;
        this.dateTime = dateTime;
    }

    public string GetText()
    {
        return text;
    }

    public long GetDateTime()
    {
        return dateTime;
    }

    public Author GetAuthor()
    {
        return _author;
    }
}