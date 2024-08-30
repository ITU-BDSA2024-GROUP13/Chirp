DateTime currentTime = DateTime.UtcNow;
long unixTime = ((DateTimeOffset)currentTime).ToUnixTimeSeconds();
DateTimeOffset.UtcNow.ToUnixTimeSeconds();
string username = Environment.UserName;

// switch (args[0])
// {
//     case "chirp":
//         Chirp(args[1], unixTime, username);
//         break;
//     default:
//         Console.WriteLine();
//         break;
// }


//Call Chirp
Chirp(args, unixTime, username);


static void Chirp(String[] args, long unixTime, String author){ 
    //Write message with relevant information
    Console.WriteLine(createMessage(author, unixTime, args));
    storeChirp("hello, world!", unixTime);
}

//Creates the message in the correct format
static String createMessage(String author, long unixTime, String[]args){
    //Baseconstruction of message
    String message;
    message = author + " @ " + Date(unixTime) + ": ";


    //Enters the input into the message
    foreach(String i in args){
        message += i + " ";
    }
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
        sw.WriteLine("{0},\"{1}\",{2}", Environment.UserName, message, unixTime);
    }	
}

static List<string> getChirps()
{ // reading input
    using var reader = new StreamReader(File.OpenRead("./resources/chirp_cli_db.csv"));

    List<string> listRows= new List<string>();

    while (!reader.EndOfStream)
    {
        listRows.Add(reader.ReadLine());
    }
    reader.Close();
    return listRows;
}



