using Chirp.CLI;
using SimpleDB;

long unixTime = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
const string pathToCSV = "./resources/chirp_cli_db.csv";
string username = Environment.UserName;

IDatabaseRepository<Cheep> database = new CSVDatabase<Cheep>(pathToCSV);

const string usage1 = @" Chirp CLI.
                        
Usage:
    dotnet run --chirp <message>
    dotnet run --read
    dotnet run (--help | --version)

Options:
    --help     Show this screen.
    --version  Show version information.
";


const string usage2 = @"Naval Fate.

Usage:
  naval_fate.exe ship new <name>...
  naval_fate.exe ship <name> move <x> <y> [--speed=<kn>]
  naval_fate.exe ship shoot <x> <y>
  naval_fate.exe mine (set|remove) <x> <y> [--moored | --drifting]
  naval_fate.exe (-h | --help)
  naval_fate.exe --version

Options:
  -h --help     Show this screen.
  --version     Show version.
  --speed=<kn>  Speed in knots [default: 10].
  --moored      Moored (anchored) mine.
  --drifting    Drifting mine.

";


UserInterface.Run(args, usage1, true, false);






