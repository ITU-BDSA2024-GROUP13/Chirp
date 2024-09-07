using SimpleDB;

namespace Chirp.CLI;
using System;
using DocoptNet;
#nullable enable

public static class UserInterface
{
    

    public static void Run(String[] args, string usage, bool help, bool exit)
    {
        
        // this will print in console if help = true
        var doc = new Docopt().Apply(
            usage,                              //String doc
            args,                               // IEnumerable`1 argv
            help,                               // Boolean help
            version: "Chirp pre-release v 2",   // Object version
                                                // Boolean optionsFirst
            exit: exit                         // Boolean exit
        )!;

        foreach (var (key, value) in doc)
            Console.WriteLine(key, value);
        
    }
    
    public static void PrintChirpsFromFile(string path)
    {
        var chirps = File.ReadLines(path).Skip(1);
        foreach (var chirp in chirps)
        {
            if(String.IsNullOrEmpty(chirp))
                continue;
            Console.WriteLine(HelperFunctions.formatFromFileToConsole(chirp));
        }
    }
    
    public static void PrintFromDatabaseToConsole<T>(IDatabaseRepository<T> database)
    {
        foreach (var cheep in database.Read())
        {
            Console.WriteLine(cheep.ToString());
        }
    
    }
    
    public static void Chirp(string username, string message, long unixTime, IDatabaseRepository<Cheep> database){ 
        //Write message with relevant information
        Cheep cheep = new Cheep(){Author = username, Message = message, Timestamp = unixTime};
        Console.WriteLine(cheep.ToString()); // may be deleted in the future
    
        database.Store(cheep);
    
    }
    
}