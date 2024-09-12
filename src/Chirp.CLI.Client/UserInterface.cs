using Chirp.CSVDB;

namespace Chirp.CLI.Client;
using System;

/**
 * <summary>
 * class <c>UserInterface</c> is responsible for everything regarding the user interface,
 * thus, any method which would directly communicate with the user are managed in this class.
 * </summary>
 */
public static class UserInterface
{
    
    //String to print with a 'help' command
    public static readonly string _usage1 = @" Chirp CLI.
                        
    Usage:
    dotnet run --chirp <message>
    dotnet run --read
    dotnet run (--help | --version)

    Options:
    --help     Show this screen.
    --version  Show version information.
    
    ";
    
    /**
     * <summary> This method iterates over an entire database for cheeps,
     * and prints each cheep-object with their <c>ToString()</c> method.
     * </summary>
     */
    public static void PrintFromDatabaseToConsole<T>(IDatabaseRepository<T> database)
    {
        foreach (var cheep in database.Read())
            if (cheep != null)
                Console.WriteLine(cheep.ToString());
    }
    
    /**
     * <summary> This method creates a new Cheep-object, writes the result in the terminal,
     * and stores the Cheep-object in a database.
     * <param name="unixTime">Timestamp for Cheep-instantiation.</param>
     * <param name="username">Author of the Cheep.</param>
     * </summary>
     */
    public static void Chirp(string username, string message, long unixTime, IDatabaseRepository<Cheep> database)
    { //Write message with relevant information
        Cheep cheep = new Cheep{Author = username, Message = message, Timestamp = unixTime};
        Console.WriteLine(cheep.ToString()); // may be deleted in the future
        database.Store(cheep);
    }
    
}