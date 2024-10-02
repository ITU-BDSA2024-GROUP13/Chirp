using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.CSVDBService;
using Microsoft.Extensions.Primitives;
using System.Numerics;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepService _service;
    public List<CheepViewModel> Cheeps { get; set; }

    public UserTimelineModel(ICheepService service)
    {
        _service = service;
    }
    
    public ActionResult OnGet(string author, int page = 0)
    {
        var pageQuery = Request.Query["page"];
        Console.WriteLine(pageQuery);
        Console.WriteLine("hello");
        if (!pageQuery.Equals("")){
            page = Int32.Parse(pageQuery[0]);
        }


        Cheeps = _service.GetCheepsFromAuthor(author, page);
        return Page();
    }
}
