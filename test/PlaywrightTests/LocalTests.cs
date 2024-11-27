using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Xunit;
namespace PlaywrightTests;

//Local host needs to be active for these tests to work
//(Or else the test can't redirect itself to the proper urls)
public class LocalTests
{


    
    [Fact]
    public async Task LocalHostTest()
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true,
        });
        var context = await browser.NewContextAsync();

        var page = await context.NewPageAsync();
        await page.GotoAsync("http://localhost:5273/");
        await page.GetByText("Public timeline", new() { Exact = true }).ClickAsync();
        await page.GetByText("2", new() { Exact = true }).ClickAsync();
    }
}