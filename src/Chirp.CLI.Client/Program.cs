using Chirp.CLI.Client;
using Chirp.CSVDB;

var unixTime = ((DateTimeOffset)DateTime.UtcNow).ToLocalTime().ToUnixTimeSeconds();

try
{
    switch (args[0])
    {
        case "--chirp":
            await UserInterface.Chirp(
                0,
                Environment.UserName,
                args[1], 
                unixTime
            );
            break;

            

        case "--read":
            await UserInterface.Read();
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