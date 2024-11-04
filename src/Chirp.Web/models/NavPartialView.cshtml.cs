using Microsoft.AspNetCore.Mvc;

namespace Chirp.Web.models;

public class NavPartialView {
    public required int Count { get; set; }
    public required int NextPage { get; set; }
    public required int PreviousPage { get; set; }
    public required int CurrentPage { get; set; }
    public required int LastPage { get; set; }
    [BindProperty( SupportsGet = true )]
    public required string? SearchName { get; set; }

    public int DefinePreviousPage(int page){
        return page == 0 ? 0 : page-1;
    }

    public int DefineLastPage(){
        double p = Count/32;
        int LastPage = (int) Math.Ceiling(p);
        return LastPage;
    }
}