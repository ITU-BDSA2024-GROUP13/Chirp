namespace Chirp.Web.models;

using Microsoft.AspNetCore.Mvc;
using Chirp.Services;
using System.Threading.Tasks;
using Chirp.Core.DTO;
using Chirp.Core.Entities;
using System.ComponentModel;
using System.Diagnostics;

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
    
    public async Task<bool> IsFollowing(string user, string author){
        var u = await _cheepService.FindSpecificAuthorByName(user);
        var f = await _cheepService.FindSpecificAuthorByName(author);

        return await _cheepService.IsFollowing(u.Id, f.Id);
    }

    public class FollowRequest
    {
        public required string Username { get; set; }
        public required string FollowName { get; set; }
        
    }
}
