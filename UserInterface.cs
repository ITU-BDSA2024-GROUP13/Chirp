using SimpleDB;

namespace Chirp.CLI;
using System;
using DocoptNet;
#nullable enable

public static class UserInterface
{
    
    public static readonly string _usage1 = @" Chirp CLI.
                        
    Usage:
    dotnet run --chirp <message>
    dotnet run --read
    dotnet run (--help | --version)

    Options:
    --help     Show this screen.
    --version  Show version information.
    
    ";
    
    [Obsolete("PrintChirpsFromFile is deprecated, " +
              "please use PrintFromDataBase instead.")]
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
            if (cheep != null)
                Console.WriteLine(cheep.ToString());
    }
    
    public static void Chirp(string username, string message, long unixTime, IDatabaseRepository<Cheep> database)
    { //Write message with relevant information
        Cheep cheep = new Cheep{Author = username, Message = message, Timestamp = unixTime};
        Console.WriteLine(cheep.ToString()); // may be deleted in the future
        
        database.Store(cheep);
    
    }
    
}