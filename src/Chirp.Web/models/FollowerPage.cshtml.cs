using Chirp.Core.DTO.AuthorDTO;
using Chirp.Core.Entities;
using Chirp.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Chirp.Web.models;

public class FollowerPage(ICheepService cheepService, SignInManager<Author> signInManager, UserManager<Author> userManager) : TimeLine(cheepService)
{
    private readonly SignInManager<Author> _signInManager = signInManager;
    private readonly UserManager<Author> _userManager = userManager;
    
    public List<AuthorDTO>? FollowingList { get; set; }


    public async Task<IActionResult> OnGetAsync(string author)
    {
        // Updates the page and retrieves the author details and posts.
        var page = UpdatePage();
        FollowingList = await _cheepService.GetFollowers(author);
        // Update the page with new data.
        UpdatePage(page);

        return Page();
    }

  }

