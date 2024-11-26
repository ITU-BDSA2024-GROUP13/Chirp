using System.Threading.Tasks;
using Microsoft.Playwright;
using Xunit;
namespace PlaywrightTests;

public class UnitTest1
{
    [Fact]
    public async Task LogInTest()
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false,
        });
        var context = await browser.NewContextAsync();

        var page = await context.NewPageAsync();
        await page.GotoAsync("https://bdsagroup013chirprazor.azurewebsites.net/?page=0");
        await page.GetByRole(AriaRole.Link, new() { Name = "Logout Log in" }).ClickAsync();
        await page.GetByPlaceholder("name@example.com").ClickAsync();
        await page.GetByPlaceholder("name@example.com").FillAsync("Test@gmail.com");
        await page.GetByPlaceholder("password").DblClickAsync();
        await page.GetByPlaceholder("password").FillAsync("Chirp123!");
        await page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
        var page1 = await context.NewPageAsync();
        await page1.GotoAsync("https://bdsagroup013chirprazor.azurewebsites.net/");
    }
    
    [Fact]
    public async Task LocalHostTest()
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false,
        });
        var context = await browser.NewContextAsync();

        var page = await context.NewPageAsync();
        await page.GotoAsync("http://localhost:5273/");
        await page.GetByText("Public timeline", new() { Exact = true }).ClickAsync();
        await page.GetByText("2", new() { Exact = true }).ClickAsync();
    }
}