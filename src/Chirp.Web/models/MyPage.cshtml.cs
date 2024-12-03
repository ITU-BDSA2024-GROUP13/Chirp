using Chirp.Core.DTO;
using Chirp.Core.Entities;
using Chirp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Chirp.Web.models;

/// <summary>
/// Represents the page model for displaying the user's timeline and handling account actions like "Forget Me."
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MyPage"/> class.
/// </remarks>
/// <param name="cheepService">The service for managing Cheep-related operations.</param>
/// <param name="signInManager">The service for managing sign-in actions.</param>
/// <param name="userManager">The service for managing user-related actions.</param>
public class MyPage(ICheepService cheepService, SignInManager<Author> signInManager, UserManager<Author> userManager) : TimeLine(cheepService)
{
    private readonly SignInManager<Author> _signInManager = signInManager;
    private readonly UserManager<Author> _userManager = userManager;
    
    /// <summary>
    /// Gets or sets the list of Cheeps (posts) for the current user.
    /// </summary>
    public required List<CheepDTO> MyCheeps { get; set; }

    /// <summary>
    /// Gets or sets the data transfer object representing the author.
    /// </summary>
    public required AuthorDTO AuthorDTO { get; set; }

    /// <summary>
    /// Gets or sets the username of the author whose page is being viewed.
    /// </summary>
    public required string Author { get; set; }


    /// <summary>
    /// Handles the GET request to display the author's page and their Cheeps.
    /// </summary>
    /// <param name="author">The username of the author to display.</param>
    /// <returns>An <see cref="IActionResult"/> representing the result of the page request.</returns>
    public async Task<IActionResult> OnGetAsync(string author)
    {
        // Updates the page and retrieves the author details and posts.
        var page = UpdatePage();
        AuthorDTO = await _cheepService.FindSpecificAuthorByName(author);
        MyCheeps = await _cheepService.ReadUserMessages(author, page);
        Count = await _cheepService.CountUserMessages(author);

        // Update the page with new data.
        UpdatePage(page);

        return Page();
    }

    /// <summary>
    /// Handles the POST request for the "Forget Me" action, which deletes the user's data.
    /// </summary>
    /// <param name="forgetMeRequest">The request containing the username to be forgotten.</param>
    /// <returns>A <see cref="JsonResult"/> indicating the success or failure of the action.</returns>
    public async Task<IActionResult> OnPostForgetMe([FromBody] ForgetMeRequest forgetMeRequest)
    {
        // Check if the request is valid.
        if (forgetMeRequest == null)
        {
            return StatusCode(500);  // Internal server error if the request is invalid.
        }

        // Find the user and perform the "Forget Me" operation.
        var user = await _cheepService.FindSpecificAuthorByName(forgetMeRequest.UserName);
        var feedback = await _cheepService.ForgetMe(user.Name);

        // Sign the user out after the operation.
        await _signInManager.SignOutAsync();

        // Return a JSON result indicating whether the operation was successful.
        return new JsonResult(new { success = feedback });
    }

    /// <summary>
    /// Represents a request to delete a user's data, typically used in the "Forget Me" action.
    /// </summary>
    public class ForgetMeRequest
    {
        /// <summary>
        /// Gets or sets the username of the user to be forgotten.
        /// </summary>
        public required string UserName { get; set; }
    }
}

