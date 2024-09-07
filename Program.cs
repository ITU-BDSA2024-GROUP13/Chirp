using Chirp.CLI;
using SimpleDB;

long unixTime = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
const string pathToCSV = "./resources/chirp_cli_db.csv";
string username = Environment.UserName;

IDatabaseRepository<Cheep> database = new CSVDatabase<Cheep>(pathToCSV);

switch (args[0])
{
    case "cheep":
        UserInterface.Chirp(username, args[1], unixTime, database);
        break;
    case "read":
        UserInterface.PrintFromDatabaseToConsole(database);
        break;
    default:
        UserInterface.Help(args, true, false);
        break;
}






