using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Xunit;
namespace PlaywrightTests;

public class Azuretests
{
    [Fact]
    public async Task LogInTest()
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true,
        });
        var context = await browser.NewContextAsync();

        var page = await context.NewPageAsync();
        await page.GotoAsync("https://bdsagroup013chirprazor.azurewebsites.net/?page=0");
        string? ActualTitle = await page.Locator("h1").TextContentAsync();
        string? ActualLogin = await page.Locator(".nav-item").TextContentAsync();

        await page.GetByRole(AriaRole.Link, new() { Name = "Logout Log in" }).ClickAsync();
        await page.GetByPlaceholder("name@example.com").ClickAsync();
        await page.GetByPlaceholder("name@example.com").FillAsync("Test@gmail.com");
        await page.GetByPlaceholder("password").DblClickAsync();
        await page.GetByPlaceholder("password").FillAsync("Chirp123!");
        await page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
        var page1 = await context.NewPageAsync();
        await page1.GotoAsync("https://bdsagroup013chirprazor.azurewebsites.net/");
        string? ActualLogout = await page1.Locator(".nav-item").TextContentAsync();

        Assert.Equal("Chirp!", ActualTitle);
        Assert.Equal("Log in", ActualLogin.Trim());
        Assert.Equal("Logout[TestName]", ActualLogout.Trim());
    }
}