using Chirp.CLI.Client;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<ICSVService, CSVService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "CheepAPI";
    config.Title = "CheepAPI v1";
    config.Version = "v1";
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "CheepAPI";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}




var cheep = new Cheep
{
    Author = "Chirp",
    Message = "Hello, World!",
    Timestamp = 1234567890
};
app.MapGet("/cheeps", () => cheep);

app.Run();
