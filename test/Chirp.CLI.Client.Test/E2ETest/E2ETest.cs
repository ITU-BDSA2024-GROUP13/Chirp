using System.Diagnostics;
using Microsoft.VisualBasic;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace E2ETest;

public class E2ETest
{
    [Fact]
    public static void Test1()
    {
        //6:24:00 PM
        string output = "";
        var cheep = new Chirp.CLI.Client.Cheep { Author = "Alex", Message = "hej", Timestamp = 1726244640 };

        using (var process = new Process())
        {

            process.StartInfo.FileName = "../../../../../../src/Chirp.CLI.Client/bin/Debug/net7.0/Chirp.CLI.Client";
            process.StartInfo.Arguments = "--read";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            //process.StartInfo.WorkingDirectory = "./";
            process.StartInfo.RedirectStandardError = true;


            process.Start();
            StreamReader reader = process.StandardOutput;
            output = reader.ReadToEnd();
            process.WaitForExit();
        }
        var split = output.Split("\n");



        Assert.StartsWith("ropf", split[0]);
        foreach (var text in split)
        {
            Console.WriteLine(text);
        }

        Assert.Equal("ropf @ 01/08/2023 14:09:20: Hello, BDSA students!", split[0].Trim());
        Assert.EndsWith("singleton", split[7].Trim());

    }
}