using System.Reflection;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

try{
    var embeddedProvider = new EmbeddedFileProvider(Assembly.GetExecutingAssembly());
    using var reader = embeddedProvider.GetFileInfo("./data/chirps.db").CreateReadStream();
    using var sr = new StreamReader(reader);
    var query = sr.ReadToEnd();
    
    var i = 0;

    foreach(var queri in query) {
        
        Console.WriteLine(++i);
        Console.WriteLine(queri);

    }
} catch (FileNotFoundException e){
    Console.WriteLine(e.Message);
}


// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton<ICheepService, CheepService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();

app.Run();
