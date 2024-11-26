using Chirp.Repositories;
using Chirp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.models;

public class MyPage(ICheepService cheepService) : TimeLine(cheepService) {

    public IActionResult OnGetAsync(string author)
    {
        // Handle the logic for this route, e.g., fetching user data
        return Page();
    }
}