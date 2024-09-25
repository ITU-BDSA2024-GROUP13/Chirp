using Chirp.CSVDB;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Encoding = System.Text.Encoding;

namespace Chirp.CLI.Client;
using System;
using System.Data.Common;

/**
 * <summary>
 * class <c>UserInterface</c> is responsible for everything regarding the user interface,
 * thus, any method which would directly communicate with the user are managed in this class.
 * </summary>
 */
public static class UserInterface
{
    // Client for HTTP-requests
    static readonly HttpClient _client = new ();
    // Base URL for HTTP-requests
    static readonly string _baseURL = "http://localhost:5000/";


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
    public static async Task Read()
    {
        try
        {
            // Connect to the server and get the response
            using HttpResponseMessage response = await _client.GetAsync($"{_baseURL}Cheeps");
            response.EnsureSuccessStatusCode();
            // read the response as a string
            var responseBody = await response.Content.ReadAsStringAsync();
            // print the response
            Console.WriteLine(responseBody);
        }
        catch (HttpRequestException e)
        {
            if (e.InnerException != null) { Console.WriteLine(e.InnerException.Message); }
            else { Console.WriteLine(e.Message); }
        }
    }

    /**
     * <summary> This method creates a new Cheep-object, writes the result in the terminal,
     * and stores the Cheep-object in a database.
     * <param name="id">Unique identifier for the Cheep-object.</param>
     * <param name="username">Author of the Cheep.</param>
     * <param name="message">Content of the Cheep.</param>
     * <param name="unixTime">Timestamp for Cheep-instantiation.</param>
     * </summary>
     */
    public static async Task Chirp(int id, string username, string message, long unixTime)
    {
        try
        {

            // Send a POST-request to the server
            using var response = await _client.PostAsync(
                $"{_baseURL}Cheeps",
                // Creates a new StringContent-object with the JSON-string of a new Cheep-object
                new StringContent(
                    // Convert the Cheep-object to a JSON-string
                    JsonSerializer.Serialize(
                        new Cheep { Id = id, Author = username, Message = message, Timestamp = unixTime }),
                        Encoding.UTF8,
                        "application/json"
                )
            );

        }
        catch (HttpRequestException e)
        {
            if (e.InnerException != null) { Console.WriteLine(e.InnerException.Message); }
            else { Console.WriteLine(e.Message); }
        }
    }

    [Obsolete("This method is deprecated, use Chirp(int id, string username, string message, long unixTime) instead.")]
    public static void Chirp(int id, string username, string message, long unixTime, IDatabaseRepository<Cheep> database)
    { //Write message with relevant information
        Cheep cheep = new() { Id = id, Author = username, Message = message, Timestamp = unixTime };
        cheep.Validate();
        Console.WriteLine(cheep.ToString());
        database.Store(cheep);
    }

}