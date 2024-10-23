using System.ComponentModel.DataAnnotations;

namespace Chirp.Repositories;

/// <summary>
/// This is a cheep object which is used to store data about a cheep in a database.
/// </summary>

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
        
    /// <summary>
    /// This method returns the text content of a cheep
    /// </summary>
    /// <returns>string</returns>

    public string GetText()
    {
        return Text;
    }
    
    /// <summary>
    /// This method returns a datetime object that represents the time the
    /// cheep was written.
    /// </summary>
    /// <returns>DateTime</returns>
    
    public DateTime GetDateTime()
    {
        return TimeStamp;
    }
    
    /// <summary>
    /// This method returns the author of the cheep.
    /// </summary>
    /// <returns>Author</returns>
    
    public Author GetAuthor()
    {
        return Author;
    }
}