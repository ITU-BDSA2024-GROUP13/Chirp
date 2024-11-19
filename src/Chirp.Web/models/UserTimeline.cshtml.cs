namespace Chirp.Web.models;

using Microsoft.AspNetCore.Mvc;
using Chirp.Services;
using System.Threading.Tasks;
using Chirp.Core.DTO;
using Chirp.Core.Entities;

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
        var user = await _cheepService.FindSpecificAuthorByName(followRequest.Username);
        var follower = await _cheepService.FindSpecificAuthorByName(followRequest.FollowName);
        
        await _cheepService.Follow(user.Id, follower.Id);
        return new JsonResult(new { success = true, message = "FollowRequest successfully processed" });
    }

    public class FollowRequest
    {
        public required string Username { get; set; }
        public required string FollowName { get; set; }
        
    }
}
