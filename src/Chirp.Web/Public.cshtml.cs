namespace Chirp.Web;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Services;
using Chirp.Repositories;
using System.Threading.Tasks;

public class PublicModel : PageModel
{
    private readonly ICheepRepository _service;
    public List<CheepDTO> Cheeps { get; set; }
    public int count {get; set; }
    public int nextPage {get; set;}
    public int previousPage {get; set;}
    public int currentPage {get; set;}

    public int lastPage {get; set;}
    public PublicModel(ICheepRepository service)
    {
        _service = service;
    }

    public int definePreviousPage(int page){
        if(page == 0){
            return 0;
        } else{
            return page-1;
        }
    }

    public int defineLastPage(){
        double p = count/32;
        int lastPage = (int) Math.Ceiling(p);
        return lastPage;
    }

    public async Task<ActionResult> OnGetAsync(int page = 0)
    {
         var pageQuery = Request.Query["page"];
        if (!pageQuery.Equals("") && pageQuery.Count() > 0){
            page = Int32.Parse(pageQuery[0]);
        }
        currentPage = page;
        nextPage = page+1;
        previousPage = definePreviousPage(page);
        lastPage = defineLastPage();
        Cheeps = await _service.ReadPublicMessages(page);
        count = await _service.CountPublicMessages();
        return Page();
    }
}
