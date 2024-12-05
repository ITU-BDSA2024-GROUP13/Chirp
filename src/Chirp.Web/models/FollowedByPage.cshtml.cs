using Chirp.Core.DTO;
using Chirp.Core.Entities;
using Chirp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Chirp.Web.models;

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

