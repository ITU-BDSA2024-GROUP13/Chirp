﻿using System.Text.RegularExpressions;

long unixTime = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();


switch (args[0])
{
    case "cheep":
        if (!args[1].StartsWith("\"") && !args[1].EndsWith("\""))
        {
            args[1] = $"\"{args[1]}\"";
        } else if (!args[1].StartsWith("\""))
        {
            args[1] = $"\"{args[1]}";
        } else if (!args[1].EndsWith("\""))
        {
            args[1] = $"{args[1]}\"";
        } // there are more edge cases but i cant be bothered rn.
        
        chirp(args[1], unixTime);
        
        break;
    case "read":
        printChirps();
        break;
}


static void chirp(string message, long unixTime){ 
    //Write message with relevant information
    Console.WriteLine(createMessage(unixTime, message));
    storeChirp(message, unixTime);
}

//Creates the message in the correct format
static String createMessage(long unixTime, string args){
    //Baseconstruction of message
    String message = Environment.UserName + " @ " + date(unixTime) + ": " + args;
    
    return message;
}

//Gets the relevant dateinformation from the epoch
static String date(long unixTime){
    DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
    dateTime = dateTime.AddSeconds(unixTime).ToLocalTime();
    return dateTime.ToString();
}

static void storeChirp(string message, long unixTime)
{ // stores cheep
    using (StreamWriter sw = File.AppendText("./resources/chirp_cli_db.csv"))
    {
        sw.WriteLine("{0},{1},{2}", Environment.UserName, message, unixTime);
    }	
}


static void printChirps()
{ // prints all saved cheeps
    var chirps = File.ReadLines("./resources/chirp_cli_db.csv").Skip(1);
    foreach (var chirp in chirps)
    {
        if(String.IsNullOrEmpty(chirp))
            continue;
        Console.WriteLine(formatFromFileToConsole(chirp));
    }
}

static string formatFromFileToConsole(string line)
{
    var regex = new Regex("^(?<author>[æøåa-zÆØÅA-z0-9_-]*),[\"\"](?<message>.*)[\"\"],(?<timeStamp>[0-9]*)$");
    var match = regex.Match(line);
    var author = match.Groups["author"];
    var message = match.Groups["message"];
    var timestamp = date(int.Parse(match.Groups["timeStamp"].Value));
    
    return String.Format("{0} @ {1}: {2}", author, timestamp, message);
}
