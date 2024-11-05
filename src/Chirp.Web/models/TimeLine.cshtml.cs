namespace Chirp.Web.models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Services;
using Chirp.Core.DTO;

public abstract class TimeLine(ICheepService cheepService) : PageModel 
{    
    protected readonly ICheepService _cheepService = cheepService;
    public required List<CheepDTO> Cheeps { get; set; }
    public required List<AuthorDTO> Authors { get; set; }
    public int Count { get; set; }
    public int NextPage { get; set; }
    public int PreviousPage { get; set; }
    public int CurrentPage { get; set; }
    public int LastPage { get; set; }
    [BindProperty( SupportsGet = true )]
    public string? SearchName { get; set; }

    public int DefinePreviousPage(int page){
        return page == 0 ? 0 : page-1;
    }

    public int DefineLastPage(){
        double p = Count/32;
        int LastPage = (int) Math.Ceiling(p);
        return LastPage;
    }

    public DateTime ToDateTime(long value){
        return Repositories.HelperFunctions.FromUnixTimeToDateTime(value);
    }

    public async void Post(){
        
        
    }
}