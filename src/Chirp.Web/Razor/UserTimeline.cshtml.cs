﻿namespace Chirp.Web.Razor;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Services;
using System.Threading.Tasks;
using Chirp.Core.DTO;

#pragma warning disable CS8604 // Possible null reference argument.


public class UserTimelineModel(ICheepService service) : PageModel
{
    private readonly ICheepService _service = service;
    public required List<CheepDTO> Cheeps { get; set; }
    public required List<AuthorDTO> Authors { get; set; }

    public int Count { get; set; }
    
    public int NextPage {get; set;}
    public int PreviousPage {get; set;}
    public int CurrentPage {get; set;}
    public int LastPage {get; set;}

    public int DefinePreviousPage(int page){
        if(page == 0){
            return 0;
        } else{
            return page-1;
        }
    }

    public int DefineLastPage(){
        double p = Count/32;
        int lastPage = (int) Math.Ceiling(p);
        return lastPage;
    }
    
    public async Task<ActionResult> OnGetAsync(string author, int page = 0)
    {
        var pageQuery = Request.Query["page"];
        if (!pageQuery.Equals("") && pageQuery.Count > 0){
            page = Int32.Parse(pageQuery[0]);
        }
        Cheeps = await _service.ReadUserMessages(author, page);
        Count = await _service.CountUserMessages(author);
        CurrentPage = page;
        NextPage = page+1;
        PreviousPage = DefinePreviousPage(page);
        LastPage = DefineLastPage();
        return Page();
    }
}