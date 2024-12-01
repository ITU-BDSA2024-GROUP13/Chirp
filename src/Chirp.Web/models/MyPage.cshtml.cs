using Chirp.Core.DTO;
using Chirp.Core.Entities;
using Chirp.Repositories;
using Chirp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.models;

public class MyPage(ICheepService cheepService, SignInManager<Author> signInManager, UserManager<Author> userManager) : TimeLine (cheepService)
{
    private readonly SignInManager<Author> _signInManager = signInManager;
    private readonly UserManager<Author> _userManager = userManager;
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

        var user = await _cheepService.FindSpecificAuthorByName(forgetMeRequest.UserName);
        var feedback = await _cheepService.ForgetMe(user.Name);

        await _signInManager.SignOutAsync();
        return new JsonResult(new { success = feedback });
    }

    public class ForgetMeRequest { 
        public required string UserName { get; set; }
    }
}