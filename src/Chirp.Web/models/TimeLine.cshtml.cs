namespace Chirp.Web.models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Services;
using Chirp.Core.DTO;
using Chirp.Repositories;

public abstract class TimeLine(ICheepService cheepService) : PageModel 
{    
    protected readonly ICheepService _cheepService = cheepService;
    public required List<CheepDTO> Cheeps { get; set; }
    public required List<AuthorDTO> Authors { get; set; }
    public int Count { get; set; }
    public int NextPage { get; set; }
    public int PreviousPage { get; set; }
    public int CurrentPage { get; set; }
    public int LastPage { get; set; }
    [BindProperty( SupportsGet = true )]
    public string? SearchName { get; set; }
    [BindProperty( SupportsGet = true )]
    public string? PostString { get; set; }

    public int DefinePreviousPage(int page){
        return page == 0 ? 0 : page-1;
    }

    public int DefineLastPage(){
        double p = Count/32;
        int LastPage = (int) Math.Ceiling(p);
        return LastPage;
    }
    
    protected int UpdatePage(int page = 0)
    {
        var pageQuery = Request.Query["page"];
        if (!pageQuery.Equals("") && pageQuery.Count > 0){
            page = Int32.Parse(pageQuery[0]);
        }
        
        CurrentPage = page;
        NextPage = page+1;
        PreviousPage = DefinePreviousPage(page);
        LastPage = DefineLastPage();

        return page;
    }

    public DateTime ToDateTime(long value){
        return Repositories.HelperFunctions.FromUnixTimeToDateTime(value);
    }

    public async Task<IActionResult> OnPostSave([FromBody] PostRequest postRequest)
    {
        if (String.IsNullOrWhiteSpace(postRequest?.PostString))
        {
            Console.WriteLine("Error: PostString was null.");
            return BadRequest("PostString cannot be null.");
        }

        DateTime currentTime = DateTime.UtcNow;
        long unixTime = ((DateTimeOffset)currentTime).ToUnixTimeMilliseconds();

        await _cheepService.CreateMessage(new CheepDTO{
            Author = postRequest.PostName,
            Text = postRequest.PostString,
            Timestamp = unixTime,
            AuthorId = 11
        });

        Console.WriteLine($"Received PostString:\nAuthor: {postRequest?.PostName}\nBody: {postRequest?.PostString}");

        return new JsonResult(new { success = true, message = "PostString successfully processed" });
    }


    public class PostRequest
    {
        public required string PostString { get; set; }
        public required string PostName { get; set; }
    }
    
}