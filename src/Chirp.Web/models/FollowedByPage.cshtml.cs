using Chirp.Core.DTO.AuthorDTO;
using Chirp.Core.Entities;
using Chirp.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Chirp.Web.models;
/// <summary>
/// Page model over information regarding a list of following authors of the user
/// </summary>
/// <param name="cheepService"></param>
/// <param name="signInManager"></param>
/// <param name="userManager"></param>
public class FollowedByPage(ICheepService cheepService, SignInManager<Author> signInManager, UserManager<Author> userManager) : TimeLine(cheepService)
{
    private readonly SignInManager<Author> _signInManager = signInManager;
    private readonly UserManager<Author> _userManager = userManager;
    
    public List<AuthorDTO>? FollowedList { get; set; }


    public async Task<IActionResult> OnGetAsync(string author)
    {
        // Updates the page and retrieves the author details and posts.
        var page = UpdatePage();
        FollowedList = await _cheepService.GetFollowedby(author);
        // Update the page with new data.
        UpdatePage(page);

        return Page();
    }

  }

