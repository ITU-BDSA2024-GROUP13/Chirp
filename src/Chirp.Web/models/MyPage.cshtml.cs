using Chirp.Core.DTO;
using Chirp.Core.Entities;
using Chirp.Repositories;
using Chirp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.models;

public class MyPage(ICheepService cheepService) : TimeLine(cheepService) 
{
    public required List<CheepDTO> MyCheeps { get; set; }
    public required AuthorDTO AuthorDTO { get; set; }
    public async Task<IActionResult> OnGetAsync(string author)
    {        
        var page = UpdatePage();
        MyCheeps = await _cheepService.ReadUserMessages(author, page);
        Count = await _cheepService.CountUserMessages(author);   
        UpdatePage(page);
        return Page();
    }

    public async Task<AuthorDTO> GetAuthorDTO(string name) {
        return await _cheepService.FindSpecificAuthorByName(name);
    }
}