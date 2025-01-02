using Chirp.Core.DTO.AuthorDTO;
using Chirp.Core.Entities;
using Chirp.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Chirp.Web.models;

/// <summary>
/// A page showcasing who has liked the cheep in question
/// </summary>
/// <param name="cheepService"></param>
/// <param name="signInManager"></param>
/// <param name="userManager"></param>
public class LikesPage(ICheepService cheepService, SignInManager<Author> signInManager, UserManager<Author> userManager) : TimeLine(cheepService)
{
    private readonly SignInManager<Author> _signInManager = signInManager;
    private readonly UserManager<Author> _userManager = userManager;
    
    public List<AuthorDTO>? LikeList { get; set; }

    public async Task<IActionResult> OnGetAsync(int cheepId)
    {
        // Updates the page and retrieves the author details and posts.
        var page = UpdatePage();
        LikeList = await _cheepService.GetAllLikers(cheepId);
        // Update the page with new data.
        UpdatePage(page);

        return Page();
    }

  }

