namespace Chirp.CLI;

public static class UserInterface
{
    
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