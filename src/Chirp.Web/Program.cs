using Chirp.Services;
using Chirp.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using AspNet.Security.OAuth.GitHub;
using Chirp.Web;
using Chirp.Core.Entities;

#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Dereference of a possibly null reference.

if (File.Exists(@"Chat.db"))
{
    File.Delete(@"Chat.db");
}

var allowOrigins = "_allowOrigins";

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAntiforgery(options => options.HeaderName = "RequestVerificationToken");

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowOrigins,
    policy =>
    {
        policy.WithOrigins("https://bdsagroup013chirprazor.azurewebsites.net",
        "http://localhost:5273/",
        "http://localhost:5000/");
    }
    );
});


builder.Services.AddDbContext<CheepDBContext>();
builder.Services.AddDefaultIdentity<Author>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<CheepDBContext>();
builder.Services.AddRazorPages();
builder.Services.AddMvc();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<ICheepRepository, CheepRepository>();
builder.Services.AddScoped<ICheepService, CheepService>();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddHsts(options => options.MaxAge = TimeSpan.FromHours(1));

var ClientSecret = "";
var ClientId = "";
if (builder.Configuration["OAUTH_CLIENT_ID"] != null){
    Console.WriteLine("hello!");
    ClientSecret = builder.Configuration["secrets.OAUTH_CLIENT_SECRET"];
    ClientId = builder.Configuration["secrets.OAUTH_CLIENT_ID"]; 
} else {
    ClientSecret = builder.Configuration["authentication:github:clientSecret"];
    ClientId = builder.Configuration["authentication:github:clientId"];
}

builder.Services.AddAuthentication()
    .AddGitHub(o =>
    {
        o.ClientId = ClientId!;
        o.ClientSecret = ClientSecret!;
        o.CallbackPath = "/signin-github";
        o.Scope.Add("user:email");
    });

var app = builder.Build();

if (app.Environment.IsProduction())
{
    app.UseHsts(); // Send HSTS headers, but only in production
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<CheepDBContext>();
        context.Database.Migrate();

        // Ensures "default" data from DbInitializer is shown on website.
        DbInitializer.SeedDatabase(context);
    }
    catch (Exception ex)
    {
        // Log any errors during seeding
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }

}


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCors(allowOrigins);
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapRazorPages();

app.Run();