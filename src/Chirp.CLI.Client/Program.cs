using Chirp.CLI.Client;
using Chirp.CSVDB;

var unixTime = ((DateTimeOffset)DateTime.UtcNow).ToLocalTime().ToUnixTimeSeconds();
var database = CSVDatabase<Cheep>.GetDatabase();

try
{
    switch (args[0])
    {
        case "--read":
            UserInterface.PrintFromDatabaseToConsole(database);
            break;

        case "--chirp":
            UserInterface.Chirp(
                Environment.UserName,
                string.Join(" ", args.Skip(1)),
                unixTime,
                database
            );
            break;
        case "--cheep": // just in case
            goto case "--chirp";
        default:
            Console.WriteLine(UserInterface._usage1);
            break;
    }
}
catch (FileNotFoundException e) { Console.WriteLine(e.ToString()); }
catch (Exception) { Console.WriteLine(UserInterface._usage1); }