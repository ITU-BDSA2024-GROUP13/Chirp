﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Razor.Model;
using Microsoft.Extensions.Primitives;
using System.Numerics;

namespace Chirp.Razor.Model.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepService _service;
    public List<CheepViewModel> Cheeps { get; set; }

    public int count { get; set; }

    public UserTimelineModel(ICheepService service)
    {
        _service = service;
    }
    
    public ActionResult OnGet(string author, int page = 0)
    {
        var pageQuery = Request.Query["page"];
        if (!pageQuery.Equals("") && pageQuery.Count() > 0){
            page = Int32.Parse(pageQuery[0]);
        }
        Cheeps = _service.GetCheepsFromAuthor(author, page);
        count = _service.CountFromAuthor(author);
        return Page();
    }
}
