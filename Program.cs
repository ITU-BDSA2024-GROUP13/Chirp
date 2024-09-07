﻿using System.Text.RegularExpressions;
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
        printChirpsFromFile(pathToCSV);
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
    
    var logMessage = $"\"{message}\"";
    using (StreamWriter sw = File.AppendText("./resources/chirp_cli_db.csv"))
    {
        
        sw.WriteLine("{0},{1},{2}", username, logMessage, unixTime);
    }

    
}


static void printChirpsFromFile(string path)
{
    var chirps = File.ReadLines(path).Skip(1);
    foreach (var chirp in chirps)
    {
        if(String.IsNullOrEmpty(chirp))
            continue;
        Console.WriteLine(formatFromFileToConsole(chirp));
    }
}

static string formatFromFileToConsole(string line)
{
    var regex = new Regex(
        "^(?<author>[æøåa-zÆØÅA-z0-9_-]*),[\"\"](?<message>.*)[\"\"],(?<timeStamp>[0-9]*)$");
    var match = regex.Match(line);
    var author = match.Groups["author"];
    var message = match.Groups["message"];
    var timestamp = HelperFunctions.FromUnixTimeToDateTime(int.Parse(match.Groups["timeStamp"].Value));
    
    return String.Format("{0} @ {1}: {2}", author, timestamp, message);
}
