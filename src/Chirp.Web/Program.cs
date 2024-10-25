using Chirp.Services;
using Chirp.Repositories;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CheepDBContext>(options => options.UseSqlite("Data Source=Chat.db"));





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