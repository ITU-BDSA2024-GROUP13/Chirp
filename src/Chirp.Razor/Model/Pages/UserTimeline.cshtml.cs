using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Razor.Model;
using Microsoft.Extensions.Primitives;
using System.Numerics;

namespace Chirp.Razor.Model.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepRepository _service;
    public List<CheepDTO> Cheeps { get; set; }

    public int count { get; set; }

    public UserTimelineModel(ICheepRepository service)
    {
        _service = service;
    }
    
    public async Task<ActionResult> OnGet(string author, int page = 0)
    {
        var pageQuery = Request.Query["page"];
        if (!pageQuery.Equals("") && pageQuery.Count() > 0){
            page = Int32.Parse(pageQuery[0]);
        }
        Cheeps = await _service.ReadUserMessages(author);
        count = await _service.CountUserMessages(author);
        return Page();
    }
}
