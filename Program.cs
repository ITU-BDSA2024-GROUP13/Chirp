long unixTime = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
string username = Environment.UserName;

switch (args[0])
{
    case "cheep":
        while (args[1][0] != '\"' || args[1][args[1].Length - 1] != '\"')
        {
            Console.WriteLine("Please write citation quotes between your message!: " + args[1]);
            args[1] = Console.ReadLine();
        }
        
        Chirp(args[1], unixTime, username);
        break;
    case "read":
        getChirps();
        break;
}


static void Chirp(string args, long unixTime, String author){ 
    //Write message with relevant information
    Console.WriteLine(createMessage(author, unixTime, args));
    storeChirp(args, unixTime);
}

//Creates the message in the correct format
static String createMessage(String author, long unixTime, string args){
    //Baseconstruction of message
    String message = author + " @ " + Date(unixTime) + ": " + args;
    
    return message;
}

//Gets the relevant dateinformation from the source
static String Date(long unixTime){
    DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
    dateTime = dateTime.AddSeconds(unixTime).ToLocalTime();
    return dateTime.ToString();
}

static void storeChirp(string message, long unixTime)
{ // storing input
    using (StreamWriter sw = File.AppendText("./resources/chirp_cli_db.csv"))
    {
        sw.WriteLine("{0},{1},{2}", Environment.UserName, message, unixTime);
    }	
}

static void getChirps()
{ // reading input
    using var reader = new StreamReader(File.OpenRead("./resources/chirp_cli_db.csv"));

    //List<string> listRows= new List<string>();
    
    while (!reader.EndOfStream)
    {
        //listRows.Add(reader.ReadLine());
        Console.WriteLine(reader.ReadLine());
    }
    reader.Close();
}



