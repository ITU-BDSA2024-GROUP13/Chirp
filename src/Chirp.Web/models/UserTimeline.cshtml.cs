namespace Chirp.Web.models;

using Microsoft.AspNetCore.Mvc;
using Chirp.Infrastructure.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Chirp.Core.Entities;

/// <summary>
/// Represents the model for the user timeline page, supporting displaying posts and following/unfollowing users.
/// </summary>
public class UserTimelineModel(ICheepService cheepService) : TimeLine(cheepService)
{

    /// <summary>
    /// Handles the GET request to display the user's timeline along with their followers' posts.
    /// </summary>
    /// <param name="author">The username of the author whose timeline is being viewed.</param>
    /// <returns>An <see cref="ActionResult"/> representing the page result.</returns>
    public async Task<ActionResult> OnGetAsync(string author)
    {

        int page = UpdatePage();

        if (User.Identity!.Name!.Equals(author)){
            // Fetch Cheeps from the user and their followers.
            Cheeps = await _cheepService.ReadUserAndFollowerMessages(author, page);

            // Count the total messages for pagination.
            Count = await _cheepService.CountUserAndFollowerMessages(author);
        } else{   
            // Fetch Cheeps from the user and their followers.
            Cheeps = await _cheepService.ReadUserMessages(author, page);

            // Count the total messages for pagination.
            Count = await _cheepService.CountUserMessages(author);
        }


        // If a search term is provided, find authors by name.
        if (!string.IsNullOrEmpty(SearchName))
        {
            Authors = await _cheepService.FindAuthorByName(SearchName);
        }

        // Update pagination information.
        UpdatePage(page);

        return Page();
    }
    

    /// <summary>
    /// Handles the POST request to follow or unfollow a user.
    /// </summary>
    /// <param name="followRequest">The request containing the usernames for the follow action.</param>
    /// <returns>A <see cref="JsonResult"/> indicating the success or failure of the follow action.</returns>
    public async Task<ActionResult> OnPostFollow([FromBody] FollowRequest followRequest)
    {
        var user = await _cheepService.FindSpecificAuthorByName(followRequest.Username);
        var follower = await _cheepService.FindSpecificAuthorByName(followRequest.FollowUser);

        try
        {
            // Determine whether to follow or unfollow based on the current state.
            var followSuccess = await IsFollowing(followRequest.Username, followRequest.FollowUser)
                ? await UnFollow(user.Id!, follower.Id!)
                : await Follow(user.Id!, follower.Id!);

            // Return success or failure message.
            return new JsonResult(new
            {
                success = followSuccess,
                message = followSuccess
                    ? $"{followRequest.Username} successfully unfollowed {followRequest.FollowUser}"
                    : $"{followRequest.Username} successfully followed {followRequest.FollowUser}"
            });
        }
        catch (Exception e)
        {
            // Log and return an error response.
            Console.WriteLine(e.Message);
            return StatusCode(500);
        }
    }

    /// <summary>
    /// Determines whether a user is following another user.
    /// </summary>
    /// <param name="userId">The ID of the user performing the check.</param>
    /// <param name="followerId">The ID of the follower.</param>
    /// <returns>A task that resolves to a boolean indicating the follow status.</returns>
    public async Task<bool> IsFollowing(string userId, string followerId)
    {
        var user = await _cheepService.FindSpecificAuthorByName(userId);
        var follower = await _cheepService.FindSpecificAuthorByName(followerId);
        return await _cheepService.IsFollowing(user.Id!, follower.Id!);
    }

    /// <summary>
    /// Sends a follow request to follow another user.
    /// </summary>
    /// <param name="userId">The ID of the user sending the follow request.</param>
    /// <param name="followerId">The ID of the user to be followed.</param>
    /// <returns>A task that resolves to a boolean indicating whether the follow was successful.</returns>
    private async Task<bool> Follow(string userId, string followerId)
    {
        try
        {
            await _cheepService.Follow(userId, followerId);
            return true;
        }
        catch (Exception e)
        {
            // Log and return failure.
            Console.WriteLine(e.Message);
            return false;
        }
    }

    /// <summary>
    /// Sends an unfollow request to stop following another user.
    /// </summary>
    /// <param name="userId">The ID of the user sending the unfollow request.</param>
    /// <param name="followerId">The ID of the user to be unfollowed.</param>
    /// <returns>A task that resolves to a boolean indicating whether the unfollow was successful.</returns>
    private async Task<bool> UnFollow(string userId, string followerId)
    {
        try
        {
            await _cheepService.Unfollow(userId, followerId);
            return true;
        }
        catch (Exception e)
        {
            // Log and return failure.
            Console.WriteLine(e.Message);
            return false;
        }
    }

    /// <summary>
    /// Represents a request to follow or unfollow a user.
    /// </summary>
    public class FollowRequest
    {
        /// <summary>
        /// Gets or sets the username of the user initiating the follow action.
        /// </summary>
        public required string Username { get; set; }

        /// <summary>
        /// Gets or sets the username of the user to be followed or unfollowed.
        /// </summary>
        public required string FollowUser { get; set; }
    }
}
