using Chirp.CLI;
using SimpleDB;

string pathToCSV = "./data/chirp_cli_db.csv";
IDatabaseRepository<Cheep> database = new CSVDatabase<Cheep>(pathToCSV);
long _unixTime = ((DateTimeOffset)DateTime.UtcNow).ToLocalTime().ToUnixTimeSeconds();


switch (args[0])
{ // not sure if this should go into the a function in UserInterface or not
    case "--read":
        UserInterface.PrintFromDatabaseToConsole(database);
        break;
            
    case "--chirp":
        UserInterface.Chirp(Environment.UserName, 
            string.Join(" ", args.Skip(1)), 
            _unixTime, 
            database
        );
        break;
    default:
        Console.WriteLine(UserInterface._usage1);
        break;
}






