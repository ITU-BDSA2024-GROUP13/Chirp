using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow.DisposeAnalysis;
using Microsoft.Playwright;
using Xunit;
using Xunit.Sdk;
namespace PlaywrightTests;
public class AzureTests : IAsyncLifetime
{
    private IPlaywright? playwright;
    private IBrowser? browser;
    private IBrowserContext? context;

    public async Task DisposeAsync()
    {
        if(browser != null){ await browser.CloseAsync(); }
        
        if(playwright != null){ playwright.Dispose(); }
    }

    public async Task InitializeAsync()
    {
        playwright = await Playwright.CreateAsync();
        browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true,
        });

        context = await browser.NewContextAsync();
    }



    [Fact]
    public async Task LoginChangeTest()
    {
        if (context !=null){
        var page1 = await context.NewPageAsync();
        await page1.GotoAsync("https://bdsagroup013chirprazor.azurewebsites.net/?page=0");
        string? ActualTitle = await page1.Locator("h1").TextContentAsync();
        string? ActualLogin = await page1.Locator(".nav-item").TextContentAsync();


        await page1.GetByRole(AriaRole.Link, new() { Name = "Logout Log in" }).ClickAsync();
        await page1.GetByPlaceholder("name@example.com").ClickAsync();
        await page1.GetByPlaceholder("name@example.com").FillAsync("Test@gmail.com");
        await page1.GetByPlaceholder("password").DblClickAsync();
        await page1.GetByPlaceholder("password").FillAsync("Chirp123!");
        await page1.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
        
        var page2 = await context.NewPageAsync();
        await page2.GotoAsync("https://bdsagroup013chirprazor.azurewebsites.net/");
        string? ActualLogout = await page2.Locator(".nav-item").TextContentAsync();

        Assert.Equal("Chirp!", ActualTitle);
        Assert.Equal("Log in", ActualLogin?.Trim());
        Assert.Equal("Logout[TestName]", ActualLogout?.Trim());
    }
    }
}