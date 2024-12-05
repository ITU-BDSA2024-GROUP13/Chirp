using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Xunit;
using Xunit.Sdk;
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
            Headless = true,
        });

        context = await browser.NewContextAsync();
    }

    [Fact]
    public async Task LocalLogin()
    {
        var page = await context!.NewPageAsync();
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
        var page = await context!.NewPageAsync();
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

    [Fact]
    public async Task LocalLogOut()
    {
        var page = await context!.NewPageAsync();
        Login(page);
        await page.GetByRole(AriaRole.Link, new() { Name = "Logout Logout[TestName]" }).ClickAsync();
        await page.GetByRole(AriaRole.Button, new() { Name = "Click here to Logout" }).ClickAsync();
        await page.GetByRole(AriaRole.Img, new() { Name = "Icon1" }).ClickAsync();
    }

    [Fact]
    public async Task LocalShowingCheeps()
    {
        var page = await context!.NewPageAsync();
        await page.GotoAsync("http://localhost:5273/");

        bool? CheepsNotVisible = await page.Locator(".cheep-author > a").IsVisibleAsync();
        string? NoCheepsMessage = await page.Locator("p").TextContentAsync();


        Login(page);

        //Waits for the last element to be loaded before it checks anything else on the page 
        // (error would occur before, beacause sometimes it checked before the page was loaded...)
        await page.Locator("footer").WaitForAsync();

        bool? CheepsVisible = await page.Locator(".cheep-author > a").First.IsVisibleAsync();

        Assert.Equal("Log in to experience new ideas on Chirp.", NoCheepsMessage?.Trim());
        Assert.True(CheepsVisible, "Cheeps are visible when logged in");
        Assert.False(CheepsNotVisible, "Cheeps are invisible when not logged in");

    }


    [Fact]
    public async Task LocalNavItems()
    {
        var page = await context!.NewPageAsync();
        await page.GotoAsync("http://localhost:5273/");
        var Timelines1 = await page.QuerySelectorAllAsync(".tag");


        Login(page);

        await page.Locator("footer").WaitForAsync();
        var Timelines2 = await page.QuerySelectorAllAsync(".tag");

        Assert.True(Timelines2.Count == 2, "Should have 2 elements: Public- & My Timeline");
        Assert.True(Timelines1.Count == 1, "Should only have 1: Public Timeline");

    }


    // HELPER METHODS FOR TESTS TO AVOID DUPLICATING CODE

    /// <summary>
    /// Takes a page and logs in on it
    /// </summary>
    /// <param name="page"></param>
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
}