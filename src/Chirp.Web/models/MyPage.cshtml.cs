using Chirp.Core.DTO;
using Chirp.Repositories;
using Chirp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.models;

public class MyPage(ICheepService cheepService) : TimeLine(cheepService) 
{
    public required List<CheepDTO> MyCheeps { get; set; }
    public required AuthorDTO AuthorDTO { get; set; }
    public IActionResult OnGetAsync()
    {
        UpdatePage();
        return Page();
    }

    public async Task<AuthorDTO> GetAuthorDTO(string name) {
        return await _cheepService.FindSpecificAuthorByName(name);
    }

    public async Task<List<CheepDTO>> GetCheeps(string name) {
        if(MyCheeps.Count > 0)
            return MyCheeps;

        
        return await _cheepService.ReadUserMessages(name, 0);
    }
}