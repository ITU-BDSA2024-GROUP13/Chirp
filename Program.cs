// DateTime currentTime = DateTime.UtcNow;
// long unixTime = ((DateTimeOffset)currentTime).ToUnixTimeSeconds();
// is a method that converts the current time to Unix time.
// Unix time is the number of seconds that have elapsed since 00:00:00 Coordinated Universal Time (UTC),
// Thursday, 1 January 1970.

//Console.WriteLine("Unix time: " + unixTime);
//Console.WriteLine("Current time: " + currentTime);
DateTime currentTime = DateTime.UtcNow;
long unixTime = ((DateTimeOffset)currentTime).ToUnixTimeSeconds();
Console.WriteLine(unixTime);
DateTimeOffset.UtcNow.ToUnixTimeSeconds();

//Call Chirp
Chirp(args, unixTime, Environment.UserName);



static void Chirp(String[] args, long unixTime, String author){ 
    //Write message with relevant information
    Console.WriteLine(createMessage(author, unixTime, args));

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




