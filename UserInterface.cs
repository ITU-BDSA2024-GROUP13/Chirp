namespace Chirp.CLI;
using System;
using DocoptNet;
#nullable enable

public static class UserInterface
{ 
    public static string usage = @"Chirp

    Usage:
      dotnet run --chirp <message>...
      dotnet run --read
      dotnet run (-h|-help)
      dotnet run --version
       
    Options:
      -h --help     Show this screen.
      --version     Show version.
      --read        Show chirps.
      --chirp       Store cheep.

";


    public static void help(String[] args, bool help, bool exit)
    {
        // this will print in console if help = true
        var x = new Docopt().Apply(
            usage,                              //String doc
            args,                               // IEnumerable`1 argv
            help,                               // Boolean help
            version: "Chirp pre-release v 2",   // Object version
                                                // Boolean optionsFirst
            exit: exit                         // Boolean exit
        )!;
        
        
        foreach (var (key, value) in x)
            Console.WriteLine("{0} = {1}", key, value);
        
    }
    
    public static void printChirpsFromFile(string path)
    {
        var chirps = File.ReadLines(path).Skip(1);
        foreach (var chirp in chirps)
        {
            if(String.IsNullOrEmpty(chirp))
                continue;
            Console.WriteLine(HelperFunctions.formatFromFileToConsole(chirp));
        }
    }
    
}