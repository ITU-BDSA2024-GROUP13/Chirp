using System.IO;

namespace Updates;

publice class Update{
public static void Commit(int big, int medium, int small)
{
    string[] arrLine = File.ReadAllLines("Updates.txt");
    arrLine[2] = arrLine[2]+1;
    File.WriteAllLines(fileName, arrLine);
}


}




