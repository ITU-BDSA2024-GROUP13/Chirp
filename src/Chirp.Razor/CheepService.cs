
using System.Data.Common;
using Chirp.CSVDBService;

public interface ICheepService
{
    public List<CheepViewModel> GetCheeps();
    public List<CheepViewModel> GetCheepsFromAuthor(string author);
}

public class CheepService : ICheepService
{

    private DBFacade db = new();
    // These would normally be loaded from a database for example
    private List<CheepViewModel> _cheeps = new();
    

    public List<CheepViewModel> GetCheeps()
    {
        return db.SELECT_ALL_MESSAGES();
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string author)
    {
        // filter by the provided author name
        return db.SELECT_MESSAGE_FROM_USER(author);
    }

    [Obsolete("This method is being replaced by the method from HelperFunctions in Chirp.CLI.Client")]
    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }

}
