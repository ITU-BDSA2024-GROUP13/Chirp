namespace Chirp.Web.Razor;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Services;
using System.Threading.Tasks;
using Chirp.Core.DTO;

#pragma warning disable CS8604 // Possible null reference argument.


public class UserTimelineModel(ICheepService cheepService) : TimeLine(cheepService)
{
    public async Task<ActionResult> OnGetAsync(string author, int page = 0)
    {
        var pageQuery = Request.Query["page"];
        if (!pageQuery.Equals("") && pageQuery.Count > 0){
            page = Int32.Parse(pageQuery[0]);
        }
        Cheeps = await _cheepService.ReadUserMessages(author, page);
        Count = await _cheepService.CountUserMessages(author);
        CurrentPage = page;
        NextPage = page+1;
        PreviousPage = DefinePreviousPage(page);
        LastPage = DefineLastPage();
        if(!string.IsNullOrEmpty(SearchName)){
            Authors = await _cheepService.FindAuthorByName(SearchName);
        }
        return Page();
    }
}
