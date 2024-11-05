namespace Chirp.Web.models;

public class CheepPartialView
{
    public required string Author { get; set; }
    public required string Body { get; set; }
    public required long Date { get; set; }
    public DateTime ToDateTime(long value){
        return Repositories.HelperFunctions.FromUnixTimeToDateTime(value);
    }
}