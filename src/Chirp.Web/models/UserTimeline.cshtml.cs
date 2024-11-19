namespace Chirp.Web.models;

using Microsoft.AspNetCore.Mvc;
using Chirp.Services;
using System.Threading.Tasks;

#pragma warning disable CS8604 // Possible null reference argument.


public class UserTimelineModel(ICheepService cheepService) : TimeLine(cheepService)
{
    
    public async Task<ActionResult> OnGetAsync(string author)
    {
        int page = UpdatePage();
    
        Cheeps = await _cheepService.ReadUserAndFollowerMessages(author, page);

        Count = Cheeps.Count;
        if(!string.IsNullOrEmpty(SearchName)){
            Authors = await _cheepService.FindAuthorByName(SearchName);
        }

        UpdatePage(page);

        return Page();
    }

    public async Task<ActionResult> OnPostFollow([FromBody] FollowRequest followRequest)
    {
        Console.WriteLine($"{followRequest.Username}\n{followRequest.FollowId}");
        return new JsonResult(new { success = true, message = "FollowRequest successfully processed" });
    }

    public class FollowRequest
    {
        public required string Username { get; set; }
        public required int FollowId { get; set; }
    }
}
