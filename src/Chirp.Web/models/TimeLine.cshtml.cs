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
    public string? sortState {get; set; } = "default";
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

            var sortQuery = Request.Query["sort"];
            if (!sortQuery.Equals("") && pageQuery.Count > 0)
            {
                sortState = sortQuery[0]!;

            }
            CurrentPage = page;
            NextPage = page + 1;
            PreviousPage = DefinePreviousPage(page);
            LastPage = DefineLastPage();
            return page;


        }
        catch (Exception e){

            Console.WriteLine("ERROR: " + e.Message);

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

    public IActionResult OnPostSort([FromBody] SortRequest sortRequest)
    {
        if (String.IsNullOrWhiteSpace(sortRequest?.SortString))
        {
            Console.WriteLine("Error: PostString was null.");
            return BadRequest("PostString cannot be null.");
        }

        sortState = sortRequest.SortString;
        return new JsonResult(new
        {
            success = true,
            message = "Sorting after " + sortRequest.SortString
        }
        );
    }

    public async Task<IActionResult> OnPostSave([FromBody] PostRequest postRequest)
    {
        if (String.IsNullOrWhiteSpace(postRequest?.PostString))
        {
            Console.WriteLine("Error: PostString was null.");
            return BadRequest("PostString cannot be null.");
        }

        var author = await _cheepService.FindSpecificAuthorByName(postRequest.PostName);

        await _cheepService.CreateMessage(new NewCheepDTO
        {
            Author = postRequest.PostName,
            Text = postRequest.PostString,
            Timestamp = HelperFunctions.FromDateTimetoUnixTime(DateTime.UtcNow),
            AuthorId = author.Id!,
        });

        return new JsonResult(new { success = true, message = "PostString successfully processed" });
    }

    public async Task<ActionResult> OnPostLike([FromBody] LikeRequest likeRequest)
    {
        var user = await _cheepService.FindSpecificAuthorByName(likeRequest.Username);
        var cheep = await _cheepService.FindSpecificCheepbyId(likeRequest.cheepId);
        int cheepId = (int)cheep.Id!;
        try
        {
            var likeSuccess = await HasLiked(user.Name!, cheepId) ? await UnLike(user.Id!, cheepId) : await LikeCheep(user.Id!, cheepId);
            return new JsonResult(new
            {
                success = likeSuccess,
                message = likeSuccess ? $"{likeRequest.Username} succesful {likeRequest.cheepId}" : $"{likeRequest.Username} unsuccesful {likeRequest.cheepId}",

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

    public async Task<bool> HasLiked(string userName, int? cheepId)
    {
        return await _cheepService.HasLiked(userName, (int)cheepId!);
    }

      public async Task<ActionResult> OnPostDislike([FromBody] DislikeRequest dislikeRequest)
    {
        var user = await _cheepService.FindSpecificAuthorByName(dislikeRequest.Username);
        var cheep = await _cheepService.FindSpecificCheepbyId(dislikeRequest.cheepId);
        int cheepId = (int)cheep.Id!;
        try
        {
            var likeSuccess = await HasDisliked(user.Name!, cheepId) ? await UnDislike(user.Id!, cheepId) : await DislikeCheep(user.Id!, cheepId);
            return new JsonResult(new
            {
                success = likeSuccess,
                message = likeSuccess ? $"{dislikeRequest.Username} succesfully undisliked {dislikeRequest.cheepId}" : $"{dislikeRequest.Username} succesfully disliked {dislikeRequest.cheepId}",
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return StatusCode(500);
        }
    }


    private async Task<bool> DislikeCheep(string userId, int cheepId)
    {
        try
        {
            await _cheepService.AddDislike(cheepId, userId);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }

    private async Task<bool> UnDislike(string userId, int cheepId)
    {
        try
        {
            await _cheepService.RemoveDislike(cheepId, userId);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }

    public async Task<bool> HasDisliked(string userName, int? cheepId)
    {
        return await _cheepService.HasDisliked(userName, (int)cheepId!);
    }

    public class LikeRequest
    {
        public required string Username { get; set; }
        public required int cheepId { get; set; }
    }
    public class DislikeRequest
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

        public class SortRequest
    {
        public required string SortString { get; set; }
    }
}