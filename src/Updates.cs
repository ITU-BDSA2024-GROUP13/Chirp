using System.IO;

namespace Updates;

publice class Update{
public static void Commit(int big, int medium, int small)
{
    string[] arrLine = File.ReadAllLines("Updates.txt");
    arrLine[0] = big;
    arrLine[1] = medium;
    arrLine[2] = small;
    File.WriteAllLines(fileName, arrLine);
}

public static void Release()
{
    string[] arrLine = File.ReadAllLines("Updates.txt");
    big = arrLine[0];
    medium = arrLine[1];
    small = arrLine[2];
    string[] readme = File.ReadAllLines("README.md");
    string[] version = readme[1].Split(".");
    readme[1]=version[0]+big+"."+version[1]+medium+"."+version[2]+small;
    File.WriteAllLines("README.md", readme);
}
}




