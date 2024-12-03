namespace Chirp.Web.models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Services;
using Chirp.Core.DTO;
using System.Threading.Tasks;
#pragma warning disable CS8604 // Possible null reference argument.

public class PublicTimeLine(ICheepService cheepService) : TimeLine(cheepService)
{

    public async Task<ActionResult> OnGetAsync()
    {
        int page = UpdatePage();

        switch (sortState)
        {
            case "oldest":
                Cheeps = await _cheepService.ReadPublicMessagesbyOldest(page);
                Count = await _cheepService.CountPublicMessages();
                break;

            case "mostLiked":
                Cheeps = await _cheepService.ReadPublicMessagesbyMostLiked(page);
                Count = await _cheepService.CountPublicMessages();
                break;

            case "relevance":
                Console.WriteLine("Reading by relevance as user: " + User!.Identity!.Name);
                Cheeps = await _cheepService.ReadPublicMessagesbyMostRelevance(page, User!.Identity!.Name);
                Count = await _cheepService.CountPublicMessages();
                break;

            default:
                Cheeps = await _cheepService.ReadPublicMessages(page);
                Count = await _cheepService.CountPublicMessages();
                break;
        }

        if (!string.IsNullOrEmpty(SearchName))
            Authors = await _cheepService.FindAuthorByName(SearchName);

        UpdatePage(page);

        return Page();
    }

}