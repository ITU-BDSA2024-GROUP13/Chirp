using Chirp.Services;
using Chirp.Repositories;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine(connectionString);
builder.Services.AddDbContext<CheepDBContext>(options => options.UseSqlite("Data Source=Chat.db"));

var filePath = "./data/chirps.db";

if (!File.Exists(filePath))
{

    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
    using (var fs = File.Create(filePath))
    {
        fs.Close();
        Console.WriteLine($"File {filePath} created.");
    }
}


if (File.Exists(filePath))
{
    using var reader = new FileStream(filePath, FileMode.Open, FileAccess.Read);
    using var sr = new StreamReader(reader);
    var query = sr.ReadToEnd();

    var i = 0;
    foreach (var queri in query)
    {
        Console.WriteLine(++i);
        Console.WriteLine(queri);
    }
}
else
{
    Console.WriteLine("File not found.");
}


builder.Services.AddRazorPages();
builder.Services.AddSingleton<ICheepService, CheepService>();

builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<ICheepRepository, CheepRepository>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    // From the scope, get an instance of our database context.
    // Through the `using` keyword, we make sure to dispose it after we are done.
    using var context = scope.ServiceProvider.GetService<CheepDBContext>();


    // Execute the migration from code.
    context.Database.Migrate();
    DbInitializer.SeedDatabase(context);

}


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();

app.Run();