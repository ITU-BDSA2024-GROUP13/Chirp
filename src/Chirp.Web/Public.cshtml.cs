namespace Chirp.Web;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Services;
using Chirp.Repositories;
using System.Threading.Tasks;
#pragma warning disable CS8604 // Possible null reference argument.

public class PublicModel(ICheepRepository service) : PageModel
{
    private readonly ICheepRepository _service = service;
    public required List<CheepDTO> Cheeps { get; set; }
    public int Count { get; set; }
    public int NextPage { get; set; }
    public int PreviousPage { get; set; }
    public int CurrentPage { get; set; }

    public int LastPage { get; set; }

    public int DefinePreviousPage(int page)
    {
        if (page == 0)
        {
            return 0;
        }
        else
        {
            return page - 1;
        }
    }

    public int DefineLastPage()
    {
        double p = Count / 32;
        int lastPage = (int)Math.Ceiling(p);
        return lastPage;
    }

    public async Task<ActionResult> OnGetAsync(int page = 0)
    {
        var pageQuery = Request.Query["page"];
        if (!pageQuery.Equals("") && pageQuery.Count > 0)
        {
            page = Int32.Parse(pageQuery[0]);
        }
        CurrentPage = page;
        NextPage = page + 1;
        PreviousPage = DefinePreviousPage(page);
        Cheeps = await _service.ReadPublicMessages(page);
        Count = await _service.CountPublicMessages();
        LastPage = DefineLastPage();
        return Page();
    }
}