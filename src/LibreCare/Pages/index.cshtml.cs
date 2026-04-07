using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LibreCare.Data;
using LibreCare.Services;
using Microsoft.EntityFrameworkCore;

namespace LibreCare.Pages;

public class IndexModel : PageModel
{
    private readonly ProfileService _profileService;

    public IndexModel(ProfileService profileService)
    {
        _profileService = profileService;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var profiles = await _profileService.GetAllAsync();

        if (!profiles.Any())
            return RedirectToPage("/Profiles/Create");

        var activeProfile = await _profileService.GetActiveAsync();

        if (activeProfile == null)
        {
            activeProfile = profiles.First();
            await _profileService.SetActiveAsync(activeProfile.Id);
        }

        return RedirectToPage("/Profiles/Dashboard", new { id = activeProfile.Id });
    }
}
