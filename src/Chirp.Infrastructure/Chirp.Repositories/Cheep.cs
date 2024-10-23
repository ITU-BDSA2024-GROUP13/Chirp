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


        
   
}