using System.ComponentModel.DataAnnotations;

namespace Chirp.Razor.Model;

public class Cheep
{
    [Key]
    public int CheepId;
    public string Text { get; set; }
    public DateTime TimeStamp { get; set; }
    public int AuthorId {get; set; }
    public Author Author { get; set; }
    
    public Cheep(string text, DateTime dateTime)
    {
        this.Text = text;
        this.TimeStamp = dateTime;
    }

        public Cheep()
    {
    }

    public string GetText()
    {
        return Text;
    }

    public DateTime GetDateTime()
    {
        return TimeStamp;
    }

    public Author GetAuthor()
    {
        return Author;
    }
}