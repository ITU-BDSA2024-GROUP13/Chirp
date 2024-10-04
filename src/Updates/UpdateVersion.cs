using System.IO;

namespace Updates;

publice class Update{
 public static void Release()
 {
    string[] arrLine = File.ReadAllLines("Updates.txt");
    big = arrLine[0];
    medium = arrLine[1];
    small = arrLine[2];
    string[] readme = File.ReadAllLines("README.md");
    string[] version = readme[1].Split(".");
    if (big != 0){
        version[0]++;
    }else if (medium != 0){
        version[1]++;
    } else if (small != 0){
        version[2]++;
    }
    readme[1] = string.Join(".", version);
    File.WriteAllLines("README.md", readme);
}

    public static void reset(){
        string[] arrLine = File.ReadAllLines("Updates.txt");
        arrLine[0] = "0";
        arrLine[1] = "0";
        arrLine[2] = "0";
        File.WriteAllLines("Updates.txt", arrLine);
    }
}