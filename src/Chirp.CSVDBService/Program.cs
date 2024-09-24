using Chirp.CLI.Client;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<CheepDb>(opt => opt.UseInMemoryDatabase("CheepList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddScoped<ICSVService, CSVService>();

//Swagger congiguration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "CheepAPI";
    config.Title = "CheepAPI v1";
    config.Version = "v1";
});

var app = builder.Build();
//if (app.Environment.IsDevelopment())
//{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.SwaggerEndpoint("/swagger/CheepAPI/swagger.json", "CheepAPI v1");
        config.DocumentTitle = "CheepAPI";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
//}




var cheep = new Cheep
{
    Id = 123,
    Author = "Chirp",
    Message = "Hello, World!",
    Timestamp = 1234567890
};

//Posts cheeps into the database context
app.MapPost("/cheeps", async (Cheep cheep, CheepDb db) =>
{
  db.Cheeps.Add(cheep);
    await db.SaveChangesAsync();

    return Results.Created($"/cheeps/{cheep.Id}", cheep);
});

app.MapGet("/cheeps", async (CheepDb db) =>
    await db.Cheeps.ToListAsync());

app.MapGet("/cheeps/{id}", async (int id, CheepDb db) =>
    await db.Cheeps.FindAsync(id)
        is Cheep cheep
            ? Results.Ok(cheep)
            : Results.NotFound());




app.Run();
