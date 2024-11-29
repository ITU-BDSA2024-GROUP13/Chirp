namespace Chirp.Web.models;

using Microsoft.AspNetCore.Mvc;
using Chirp.Services;
using System.Threading.Tasks;


public class UserTimelineModel(ICheepService cheepService) : TimeLine(cheepService)
{
    public async Task<ActionResult> OnGetAsync(string author)
    {
        int page = UpdatePage();
        Console.WriteLine(author+ " " + page);
        Cheeps = await _cheepService.ReadUserAndFollowerMessages(author, page);

        Count = await _cheepService.CountUserAndFollowerMessages(author);
        if (!string.IsNullOrEmpty(SearchName))
        {
            Authors = await _cheepService.FindAuthorByName(SearchName);
        }

        UpdatePage(page);

        return Page();
    }



    public async Task<ActionResult> OnPostFollow([FromBody] FollowRequest followRequest)
    {
        var user = await _cheepService.FindSpecificAuthorByName(followRequest.Username);
        var follower = await _cheepService.FindSpecificAuthorByName(followRequest.FollowUser);

        try
        {
            var followSuccess = await IsFollowing(followRequest.Username, followRequest.FollowUser) ? await UnFollow(user.Id!, follower.Id!) : await Follow(user.Id!, follower.Id!);
            return new JsonResult(new
            {
                success = followSuccess,
                message = followSuccess ? $"{followRequest.Username} succesfully followed {followRequest.FollowUser}" : $"{followRequest.Username} succesfully followed {followRequest.FollowUser}"
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return StatusCode(500);
        }
    }


    public async Task<bool> IsFollowing(string userId, string followerId)
    {
        var user = await _cheepService.FindSpecificAuthorByName(userId);
        var follower = await _cheepService.FindSpecificAuthorByName(followerId);
        return await _cheepService.IsFollowing(user.Id!, follower.Id!);
    }

    private async Task<bool> Follow(string userId, string followerId)
    {
        try
        {
            await _cheepService.Follow(userId, followerId);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }

    private async Task<bool> UnFollow(string userId, string followerId)
    {
        try
        {
            await _cheepService.Unfollow(userId, followerId);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }

    public class FollowRequest
    {
        public required string Username { get; set; }
        public required string FollowUser { get; set; }
    }
}