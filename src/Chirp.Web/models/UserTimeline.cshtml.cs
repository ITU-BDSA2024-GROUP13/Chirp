namespace Chirp.Web.models;

using Microsoft.AspNetCore.Mvc;
using Chirp.Services;
using System.Threading.Tasks;


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
        Console.WriteLine(followRequest.FollowUser + "\n" + followRequest.Username);
        var user = await _cheepService.FindSpecificAuthorByName(followRequest.Username);
        var follower = await _cheepService.FindSpecificAuthorByName(followRequest.FollowUser);

        try{
            var followSuccess = followRequest.Follow ? await Follow(user.Id, follower.Id) : await UnFollow(user.Id, follower.Id);
            Console.WriteLine("success");
            
            return new JsonResult(new { 
            success = followSuccess, 
            message = "FollowRequest successfully processed" 
            });
        } catch (Exception e) {
            Console.WriteLine(e.Message);
            return StatusCode(500);
        }        
    }

    /*
    public async Task<ActionResult> OnGetIsFollow()
    {
        var user = await _cheepService.FindSpecificAuthorByName(Username);
        var follower = await _cheepService.FindSpecificAuthorByName(FollowName);
        return new JsonResult(new { 
            success = await IsFollowing(user.Id, follower.Id)
        });
    }*/
    
    private async Task<bool> IsFollowing(int userId, int followerId)
    {
        return await _cheepService.IsFollowing(userId, followerId);
    }

    private async Task<bool> Follow(int userId, int followerId)
    {
        try{
            await _cheepService.Follow(userId, followerId);
            return true;
        } catch (Exception e) {
            Console.WriteLine(e.Message);
            return false;
        }
    }

    private async Task<bool> UnFollow(int userId, int followerId)
    {
        try{
            await _cheepService.Unfollow(userId, followerId);
            return true;
        } catch (Exception e) {
            Console.WriteLine(e.Message);
            return false;
        }
    }

    public class FollowRequest
    {
        public required bool Follow { get; set; }
        public required string Username { get; set; }
        public required string FollowUser { get; set; }
    }
}
