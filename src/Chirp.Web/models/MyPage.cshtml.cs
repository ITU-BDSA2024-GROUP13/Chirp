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
    public required string Author { get; set; }
    public async Task<IActionResult> OnGetAsync(string author)
    {        
        var page = UpdatePage();
        AuthorDTO = await _cheepService.FindSpecificAuthorByName(author);
        MyCheeps = await _cheepService.ReadUserMessages(author, page);
        Count = await _cheepService.CountUserMessages(author);   
        UpdatePage(page);
        return Page();
    }

    public async Task<IActionResult> OnPostForgetMe([FromBody] ForgetMeRequest forgetMeRequest)
    {
        if(forgetMeRequest == null) {
            return StatusCode(500);
        }

        var feedback = await _cheepService.ForgetMe(forgetMeRequest.UserName);

        return new JsonResult(new { success = feedback });
    }

    public class ForgetMeRequest { 
        public required string UserName { get; set; }
    }
}