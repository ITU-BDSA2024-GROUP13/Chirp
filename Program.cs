using System.Text.RegularExpressions;
using Chirp.CLI;



long unixTime = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
const string pathToCSV = "./resources/chirp_cli_db.csv";
string username = Environment.UserName;

switch (args[0])
{
    case "cheep":
        chirp(username, args[1], unixTime);
        break;
    case "read":
        UserInterface.printChirpsFromFile(pathToCSV);
        break;
}

static void chirp(string username, string message, long unixTime){ 
    //Write message with relevant information
    Console.WriteLine(formatMessage(username, unixTime, message)); // may be deleted in the future
    storeChirpToFile(username, message, unixTime, pathToCSV);
}

//Creates the message in the correct format
static String formatMessage(string username, long unixTime, string args){
    //Baseconstruction of message
    String message = username + " @ " + HelperFunctions.FromUnixTimeToDateTime(unixTime) + ": " + args;
    return message;
}

//Gets the relevant dateinformation from the epoch


static void storeChirpToFile(string username, string message, long unixTime, String path)
{
    using (StreamWriter sw = File.AppendText("./resources/chirp_cli_db.csv"))
        sw.WriteLine("{0},{1},{2}", username, message, unixTime);
}


