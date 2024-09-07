using System.Text.RegularExpressions;
using Chirp.CLI;
using SimpleDB;


long unixTime = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
const string pathToCSV = "./resources/chirp_cli_db.csv";
string username = Environment.UserName;

IDatabaseRepository<Cheep> database = new CSVDatabase<Cheep>(pathToCSV);

switch (args[0])
{
    case "cheep":
        chirp(username, args[1], unixTime, database);
        break;
    case "read":
        printFromDatabaseToConsole(database);
        //printChirpsFromFile(pathToCSV);
        break;
}
static void chirp(string username, string message, long unixTime, IDatabaseRepository<Cheep> database){ 
    //Write message with relevant information
    Cheep cheep = new Cheep(){Author = username, Message = message, Timestamp = unixTime};
    Console.WriteLine(cheep.ToString()); // may be deleted in the future
    
    database.Store(cheep);
    // storeChirpToFile(username, message, unixTime, pathToCSV);
}



static void printFromDatabaseToConsole(IDatabaseRepository<Cheep> database)
{
    foreach (var cheep in database.Read())
    {
        Console.WriteLine(cheep.ToString());
    }
    
}


