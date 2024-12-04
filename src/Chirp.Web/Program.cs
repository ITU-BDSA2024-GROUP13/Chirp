using Chirp.Services;
using Chirp.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using AspNet.Security.OAuth.GitHub;
using Chirp.Web;
using Chirp.Core.Entities;
using System.Reflection;

#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Dereference of a possibly null reference.

if (File.Exists(@"Chat.db"))
{
    File.Delete(@"Chat.db");
}

var allowOrigins = "_allowOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.Sources.Clear();

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    //.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    //.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddUserSecrets(Assembly.GetExecutingAssembly(), true); // Adds the secrets to the builder.Configuration.

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


builder.Services.AddAuthentication()
    .AddGitHub(o =>
    {
        o.ClientId = builder.Configuration["OAUTH_CLIENT_CLIENTID"] ?? throw new NullReferenceException("ClientId cannot be null");
        o.ClientSecret = builder.Configuration["OAUTH_CLIENT_SECRET"] ?? throw new NullReferenceException("ClientSecret cannot be null");
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