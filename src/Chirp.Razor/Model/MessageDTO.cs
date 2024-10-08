namespace Chirp.Razor.Model;

public class MessageDTO
{
    public long timestamp {get; set; }

    public string text{get; set; }

    public string author {get; set; }

    public int authorId {get; set; }

}