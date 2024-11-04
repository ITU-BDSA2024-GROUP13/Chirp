namespace Chirp.Web.models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Services;
using Chirp.Core.DTO;
using System.Threading.Tasks;
#pragma warning disable CS8604 // Possible null reference argument.

public class PublicTimeLine(ICheepService cheepService) : TimeLine(cheepService)
{

    public async Task<ActionResult> OnGetAsync(int page = 0)
    {
         var pageQuery = Request.Query["page"];
        if (!pageQuery.Equals("") && pageQuery.Count > 0){
            page = Int32.Parse(pageQuery[0]);
        }
        CurrentPage = page;
        NextPage = page+1;
        PreviousPage = DefinePreviousPage(page);
        Cheeps = await _cheepService.ReadPublicMessages(page);
        Count = await _cheepService.CountPublicMessages();
        if(!string.IsNullOrEmpty(SearchName)){
            Authors = await _cheepService.FindAuthorByName(SearchName);
        }

        LastPage = DefineLastPage();
        return Page();
    }
    
}
