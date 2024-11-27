using Chirp.Services;
using Microsoft.AspNetCore.Mvc;
using static Chirp.Web.models.UserTimelineModel;

namespace Chirp.Web.models;

public class CheepPartialView(ICheepService cheepService)
{
    private readonly ICheepService _cheepService = cheepService;
    public required int Id { get; set; }
    public required string Author { get; set; }
    public required string Body { get; set; }
    public required long Date { get; set; }
    public required int Like { get; set; }
    public DateTime ToDateTime(long value)
    {
        return Repositories.HelperFunctions.FromUnixTimeToDateTime(value);
    }

    public async Task<ActionResult> OnPostFollow([FromBody] LikeRequest likeRequest)
    {
        var user = await _cheepService.FindSpecificAuthorByName(likeRequest.Username);
        var cheep = await _cheepService.FindSpecificCheepbyId(likeRequest.cheepId);

        try
        {
            var followSuccess = await HasLiked(likeRequest.Username, likeRequest.cheepId) ? await UnLike(user.Id!, cheep ) : await Follow(user.Id!, follower.Id!);
            return new JsonResult(new
            {
                success = followSuccess,
                message = followSuccess ? $"{likeRequest.Username} succesfully followed {followRequest.FollowUser}" : $"{followRequest.Username} succesfully followed {followRequest.FollowUser}"
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

    public async Task<bool> HasLiked(string userId, int cheepId)
    {
        return await _cheepService.HasLiked(userId, cheepId);
    }

    public class LikeRequest
    {
        public required string Username { get; set; }
        public required int cheepId { get; set; }
    }
    
}