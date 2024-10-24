namespace Chirp.Services;

using Chirp.Repositories;

public class CheepService : ICheepService
{

    private DBFacade db = new();
    // These would normally be loaded from a database for example
    private List<CheepViewModel> _cheeps = new();


    public List<CheepViewModel> GetCheeps(int page)
    {
        return db.SELECT_ALL_MESSAGES(page);
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string author, int page)
    {
        // filter by the provided author name
        return db.SELECT_MESSAGE_FROM_USER(author, page);
    }

    public int CountFromAuthor(string author)
    {
        // filter by the provided author name
        return db.COUNT_MESSAGE_FROM_USER(author);
    }

    public int CountFromAll()
    {
        // filter by the provided author name
        return db.COUNT_MESSAGE_FROM_ALL();
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