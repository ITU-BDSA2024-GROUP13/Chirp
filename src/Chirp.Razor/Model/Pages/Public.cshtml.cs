using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Razor.Model;

namespace Chirp.Razor.Model.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepRepository _service;
    public List<CheepDTO> Cheeps { get; set; }
    public int count {get; set; }


    public PublicModel(ICheepRepository service)
    {
        _service = service;
    }

    public async Task<ActionResult> OnGet(int page = 0)
    {
        var pageQuery = Request.Query["page"];
        if (!pageQuery.Equals("") && pageQuery.Count() > 0){
            page = Int32.Parse(pageQuery[0]);
        }
        Cheeps = await _service.ReadPublicMessages(page);
        count = await _service.CountPublicMessages();
        return Page();
    }
}
