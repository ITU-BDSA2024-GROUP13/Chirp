using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Infrastructure.Services;
using Chirp.Core.DTO.CheepDTO;
using Chirp.Core.DTO.AuthorDTO;
using System.Threading.Tasks;

namespace Chirp.Web.models;

/// <summary>
/// Represents the public timeline page model, which displays public Cheeps and supports sorting and search functionality.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="PublicTimeLine"/> class.
/// </remarks>
/// <param name="cheepService">The service that handles operations related to Cheeps.</param>
public class PublicTimeLine(ICheepService cheepService) : TimeLine(cheepService)
{

    /// <summary>
    /// Handles the GET request to display the public timeline with various sorting options and search functionality.
    /// </summary>
    /// <returns>An <see cref="ActionResult"/> representing the result of the page request.</returns>
    public async Task<ActionResult> OnGetAsync()
    {
        // Get the current page for pagination.
        int page = UpdatePage();

        // Switch based on the selected sorting option.
        switch (SortState)
        {
            case "oldest":
                // Load Cheeps sorted by oldest.
                Cheeps = await _cheepService.ReadPublicMessagesbyOldest(page);
                Count = await _cheepService.CountPublicMessages();
                break;

            case "mostLiked":
                // Load Cheeps sorted by most liked.
                Cheeps = await _cheepService.ReadPublicMessagesbyMostLiked(page);
                Count = await _cheepService.CountPublicMessages();
                break;

            case "relevance":
                // Load Cheeps sorted by relevance to the current user.
                Cheeps = await _cheepService.ReadPublicMessagesbyMostRelevance(page, User!.Identity!.Name!);
                Count = await _cheepService.CountPublicMessages();
                break;

            default:
                // Load Cheeps with default sorting (most recent).
                Cheeps = await _cheepService.ReadPublicMessages(page);
                Count = await _cheepService.CountPublicMessages();
                break;
        }

        // If a search name is provided, find authors by name.
        if (!string.IsNullOrEmpty(SearchName))
            Authors = await _cheepService.FindAuthorByName(SearchName);

        // Update the page number based on pagination.
        UpdatePage(page);

        // Return the page.
        return Page();
    }
}
