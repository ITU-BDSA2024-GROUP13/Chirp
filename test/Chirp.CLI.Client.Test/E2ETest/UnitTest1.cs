using System.Diagnostics;
using Microsoft.VisualBasic;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace E2ETest;

public class UnitTest1
{
    [Fact]
    public static void Test1()
    {
        //6:24:00 PM
        string output = "";
        var cheep = new Chirp.CLI.Client.Cheep{Author = "Alex", Message = "hej", Timestamp = 1726244640 };

        using (var process = new Process()){
        
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
        Assert.EndsWith("singleton", split[split.Length-2]);
    
    }
}