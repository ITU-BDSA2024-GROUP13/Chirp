using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Xunit;
namespace PlaywrightTests;

//Local host needs to be active for these tests to work
//(Or else the test can't redirect itself to the proper urls)
public class LocalTests : IAsyncLifetime
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
            Headless = false,
        });

        context = await browser.NewContextAsync();
    }

    internal async void Login(IPage page)
    {
        await page.GotoAsync("http://localhost:5273/");
        await page.GetByRole(AriaRole.Link, new() { Name = "Logout Log in" }).ClickAsync();
        await page.GetByPlaceholder("name@example.com").ClickAsync();
        await page.GetByPlaceholder("name@example.com").FillAsync("Test@gmail.com");
        await page.GetByPlaceholder("password").ClickAsync();
        await page.GetByPlaceholder("password").FillAsync("Chirp123!");
        await page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
    }

    [Fact]
    public async Task LocalLogin()
    {
        var page = await context.NewPageAsync();
        await page.GotoAsync("http://localhost:5273/");
        await page.GetByRole(AriaRole.Link, new() { Name = "Logout Log in" }).ClickAsync();
        await page.GetByPlaceholder("name@example.com").ClickAsync();
        await page.GetByPlaceholder("name@example.com").FillAsync("Test@gmail.com");
        await page.GetByPlaceholder("password").ClickAsync();
        await page.GetByPlaceholder("password").FillAsync("Chirp123!");
        await page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
    }


    [Fact]
    public async Task LocalLoginChange()
    {
        var page = await context.NewPageAsync();
        await page.GotoAsync("http://localhost:5273/");
        string? ActualTitle = await page.Locator("h1").TextContentAsync();
        string? ActualLogin = await page.Locator(".nav-item").TextContentAsync();
        await page.GetByRole(AriaRole.Link, new() { Name = "Logout Log in" }).ClickAsync();
        await page.GetByPlaceholder("name@example.com").ClickAsync();
        await page.GetByPlaceholder("name@example.com").FillAsync("Test@gmail.com");
        await page.GetByPlaceholder("password").ClickAsync();
        await page.GetByPlaceholder("password").FillAsync("Chirp123!");
        await page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();

        var page2 = await context.NewPageAsync();
        await page2.GotoAsync("http://localhost:5273/");
        string? ActualLogout = await page2.Locator(".nav-item").TextContentAsync();
        Assert.Equal("Chirp!", ActualTitle);
        Assert.Equal("Log in", ActualLogin?.Trim());
        Assert.Equal("Logout[TestName]", ActualLogout?.Trim());
    }


}