using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading.Tasks;
using Chirp.Web.Areas.Identity.Pages.Account;
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
        if (browser != null) { await browser.CloseAsync(); }

        if (playwright != null) { playwright.Dispose(); }
    }

    public async Task InitializeAsync()
    {
        playwright = await Playwright.CreateAsync();
        browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            //To see what the tests "do" you can set this to false
            //and then you can see how it traverses through the website
            Headless = true,
        });

        context = await browser.NewContextAsync();
    }

    internal async void Login(IPage page)
    {
        await page.GotoAsync("https://bdsagroup013chirprazor.azurewebsites.net/?page=0");
        await page.GetByRole(AriaRole.Link, new() { Name = "Logout Log in" }).ClickAsync();
        await page.GetByPlaceholder("name@example.com").ClickAsync();
        await page.GetByPlaceholder("name@example.com").FillAsync("Test@gmail.com");
        await page.GetByPlaceholder("password").DblClickAsync();
        await page.GetByPlaceholder("password").FillAsync("Chirp123!");
        await page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
    }



    [Fact]
    public async Task LoginTest()
    {
        var page1 = await context.NewPageAsync();
        await page1.GotoAsync("https://bdsagroup013chirprazor.azurewebsites.net/?page=0");
        await page1.GetByRole(AriaRole.Link, new() { Name = "Logout Log in" }).ClickAsync();
        await page1.GetByPlaceholder("name@example.com").ClickAsync();
        await page1.GetByPlaceholder("name@example.com").FillAsync("Test@gmail.com");
        await page1.GetByPlaceholder("password").DblClickAsync();
        await page1.GetByPlaceholder("password").FillAsync("Chirp123!");
        await page1.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
        var page2 = await context.NewPageAsync();
        await page2.GotoAsync("https://bdsagroup013chirprazor.azurewebsites.net/");
    }

    [Fact]
    public async Task LoginChanges()
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

    [Fact]
    public async Task LogOut()
    {   
        var page = await context.NewPageAsync();
        Login(page);
        await page.GetByRole(AriaRole.Link, new() { Name = "Logout Logout[TestName]" }).ClickAsync();
        await page.GetByRole(AriaRole.Button, new() { Name = "Click here to Logout" }).ClickAsync();
        await page.GetByRole(AriaRole.Img, new() { Name = "Icon1" }).ClickAsync();
    }
}