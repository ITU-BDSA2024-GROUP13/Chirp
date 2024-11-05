namespace Chirp.Web.models;

using Microsoft.AspNetCore.Mvc;
using Chirp.Services;
using System.Threading.Tasks;

#pragma warning disable CS8604 // Possible null reference argument.


public class UserTimelineModel(ICheepService cheepService) : TimeLine(cheepService)
{
    public async Task<ActionResult> OnGetAsync(string author, int page = 0)
    {

        Cheeps = await _cheepService.ReadUserMessages(author, page);
        Count = await _cheepService.CountUserMessages(author);
        if(!string.IsNullOrEmpty(SearchName)){
            Authors = await _cheepService.FindAuthorByName(SearchName);
        }

        UpdatePage(page);

        return Page();
    }
}
