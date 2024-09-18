using Chirp.CLI.Client;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();




var cheep = new Cheep
{
    Author = "Chirp",
    Message = "Hello, World!",
    Timestamp = 1234567890
};
app.MapGet("/", () => cheep.ToString());

app.Run();
