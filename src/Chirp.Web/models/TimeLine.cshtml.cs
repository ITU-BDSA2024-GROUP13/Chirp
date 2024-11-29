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
    [BindProperty(SupportsGet = true)]
    public string? SearchName { get; set; }
    [BindProperty(SupportsGet = true)]
    public List<AuthorDTO>? SearchQuery { get; set; }
    public int DefinePreviousPage(int page)
    {
        return page == 0 ? 0 : page - 1;
    }

    public int DefineLastPage()
    {
        double p = Count / 32;
        int LastPage = (int)Math.Ceiling(p);
        return LastPage;
    }

    protected int UpdatePage(int page = 0)
    {
        try {

            var pageQuery = Request.Query["page"];
            if (!pageQuery.Equals("") && pageQuery.Count > 0)
            {
                page = Int32.Parse(pageQuery[0]!);
            }

            CurrentPage = page;
            NextPage = page + 1;
            PreviousPage = DefinePreviousPage(page);
            LastPage = DefineLastPage();
            return page;


        }
        catch (Exception e){

            Console.WriteLine(e.Message);

        }

        return 0;
    }

    public DateTime ToDateTime(long value)
    {
        return Repositories.HelperFunctions.FromUnixTimeToDateTime(value);
    }

    public async Task<IActionResult> OnPostSearch([FromBody] SearchRequest searchRequest)
    {
        if (String.IsNullOrWhiteSpace(searchRequest?.SearchString))
        {
            Console.WriteLine("Error: PostString was null.");
            return BadRequest("PostString cannot be null.");
        }

        return new JsonResult(new
        {
            list = new[]{
            await _cheepService.FindAuthors(searchRequest.SearchString)
        }
        });
    }

    public async Task<IActionResult> OnPostSave([FromBody] PostRequest postRequest)
    {
        if (String.IsNullOrWhiteSpace(postRequest?.PostString))
        {
            Console.WriteLine("Error: PostString was null.");
            return BadRequest("PostString cannot be null.");
        }

        var author = await _cheepService.FindSpecificAuthorByName(postRequest.PostName);

        await _cheepService.CreateMessage(new CheepDTO
        {
            Author = postRequest.PostName,
            Text = postRequest.PostString,
            Timestamp = HelperFunctions.FromDateTimetoUnixTime(DateTime.UtcNow),
            AuthorId = author.Id!,
            Likes = 0
        });

        return new JsonResult(new { success = true, message = "PostString successfully processed" });
    }

    public async Task<ActionResult> OnPostLike([FromBody] LikeRequest likeRequest)
    {
        Console.WriteLine("HELLO??");
        var user = await _cheepService.FindSpecificAuthorByName(likeRequest.Username);
        var cheep = await _cheepService.FindSpecificCheepbyId(likeRequest.cheepId);
        Console.WriteLine("user and cheep initialized?? " + user.Name + " " + cheep.Id);

        int cheepId = (int)cheep.Id!;
        Console.WriteLine("cheepId initialized");
        var hasliked = await HasLiked(user.Id!, cheepId);
        Console.WriteLine(hasliked);


        try
        {
            var followSuccess = await HasLiked(user.Id!, cheepId) ? await UnLike(user.Id!, cheepId) : await LikeCheep(user.Id!, cheepId);
            Console.WriteLine(followSuccess);

            return new JsonResult(new
            {
                success = followSuccess,
                message = followSuccess ? $"{likeRequest.Username} succesfully unliked {likeRequest.cheepId}" : $"{likeRequest.Username} succesfully liked {likeRequest.cheepId}"
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return StatusCode(500);
        }
    }


    private async Task<bool> LikeCheep(string userId, int cheepId)
    {
        Console.WriteLine("Liking..");

        try
        {
            await _cheepService.AddLike(cheepId, userId);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }


    private async Task<bool> UnLike(string userId, int cheepId)
    {
        Console.WriteLine("UnLiking..");

        try
        {
            await _cheepService.RemoveLike(cheepId, userId);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }

    public async Task<bool> HasLiked(string userId, int cheepId)
    {
        return await _cheepService.HasLiked(userId, cheepId);
    }

    public class LikeRequest
    {
        public required string Username { get; set; }
        public required int cheepId { get; set; }
    }

    public class PostRequest
    {
        public required string PostString { get; set; }
        public required string PostName { get; set; }

    }

    public class SearchRequest
    {
        public required string SearchString { get; set; }
    }
}