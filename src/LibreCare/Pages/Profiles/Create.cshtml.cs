using LibreCare.Data;
using LibreCare.Models;
using LibreCare.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LibreCare.Pages.Profiles;

public class CreateModel : PageModel
{
    private readonly ProfileService _profileService;

    public CreateModel(ProfileService profileService)
    {
        _profileService = profileService;
    }

    [BindProperty] public Profile Profile { get; set; } = new();

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        // MVP: always make new profile active
        Profile.IsActive = true;

        await _profileService.CreateAsync(Profile);

        return RedirectToPage("Index");
    }
}
