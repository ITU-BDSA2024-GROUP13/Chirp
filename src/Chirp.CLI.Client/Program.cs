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
// error in filepath from database
catch (FileNotFoundException e) { Console.WriteLine(e.ToString()); }
// error in switch statement from terminal input
catch (IndexOutOfRangeException) { Console.WriteLine(UserInterface._usage1); }
// unknown error from anywhere in program
catch (Exception e) { Console.WriteLine($"Unknown Exception:\n{e.Message}"); }