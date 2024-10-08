using System.Reflection;
using Microsoft.Extensions.FileProviders;
using Chirp.Razor;

var builder = WebApplication.CreateBuilder(args);

// Reflection
var embeddedProvider = new EmbeddedFileProvider(Assembly.GetExecutingAssembly());
var fileInfo = embeddedProvider.GetFileInfo("data.schema.sql");
using var reader = fileInfo.CreateReadStream();
using var sr = new StreamReader(reader);
var content = sr.ReadToEnd();

// Read content from db and print

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton<ICheepService, CheepService>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();


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
