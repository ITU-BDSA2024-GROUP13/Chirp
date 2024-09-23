using System.Diagnostics;
using Microsoft.VisualBasic;
using Microsoft.VisualStudio.TestPlatform;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace E2ETest;

public class E2ETest
{
    [Fact]
    public static void Test1()
    {
        string output = "";

        /*
            "Provides access to local and remote processes and enables
            you to start and stop local system processes."

            var proceses = new Process() gives acess to the processes within
            a system. (based on filepath)
        */
        using (var process = new Process()){
        
            //Sets the filepath and function call to that filepath
            process.StartInfo.FileName = "../../../../../../src/Chirp.CLI.Client/bin/Debug/net7.0/Chirp.CLI.Client";
            process.StartInfo.Arguments = "--read";

            //Given the value false, the process will not use the operating system shell.
            process.StartInfo.UseShellExecute = false;

            //Given the value true, the textual output will be given/written to the process.StandardOutput stream
            process.StartInfo.RedirectStandardOutput = true;
            
            //This starts the process in the filepath with the specified function (argument)
            process.Start();
            //Collects the ouput given by the process
            StreamReader reader = process.StandardOutput;
            output = reader.ReadToEnd();
            process.WaitForExit();
        }
        //Splits by each line
        var split = output.Split("\n");
        
        //Tests that the first word and sentence given by the process and printed
        //matches the expected outcome
        Assert.StartsWith("ropf", split[0]);
        Assert.Equal("ropf @ 8/1/2023 2:09:20PM: Hello, BDSA students!", split[0].Trim());
        
        //Tests that the last word and sentence given by the process and printed
        //matches the expected outcome
        Assert.EndsWith("singleton", split[7].Trim());
        Assert.Equal("andrebirk @ 9/12/2024 12:31:46AM: Testing singleton", split[7].Trim());
    }
}