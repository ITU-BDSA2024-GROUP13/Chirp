using Microsoft.AspNetCore.Mvc;

namespace Chirp.Web.models;

public class NavBarPartialView {
    public int Count { get; set; }
    public int NextPage { get; set; }
    public int PreviousPage { get; set; }
    public int CurrentPage { get; set; }
    public int LastPage { get; set; }
    [BindProperty( SupportsGet = true )]
    public string? SearchName { get; set; }

    public int DefinePreviousPage(int page){
        if(page == 0){
            return 0;
        } else{
            return page-1;
        }
    }

    public int DefineLastPage(){
        double p = Count/32;
        int LastPage = (int) Math.Ceiling(p);
        return LastPage;
    }
}