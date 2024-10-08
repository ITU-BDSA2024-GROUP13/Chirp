using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Razor.Model;

namespace Chirp.Razor.Model.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepService _service;
    public List<CheepViewModel> Cheeps { get; set; }
    public int count {get; set; }


    public PublicModel(ICheepService service)
    {
        _service = service;
    }

    public ActionResult OnGet(int page = 0)
    {
        var pageQuery = Request.Query["page"];
        if (!pageQuery.Equals("") && pageQuery.Count() > 0){
            page = Int32.Parse(pageQuery[0]);
        }
        Cheeps = _service.GetCheeps(page);
        count = _service.CountFromAll();
        return Page();
    }
}
