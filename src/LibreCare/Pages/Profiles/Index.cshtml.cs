using LibreCare.Data;
using LibreCare.Models;
using LibreCare.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LibreCare.Pages.Profiles;

public class IndexModel : PageModel
{
    private readonly ProfileService _profileService;

    public IndexModel(ProfileService profileService)
    {
        _profileService = profileService;
    }

    public List<Profile> Profiles { get; set; } = new();

    public async Task OnGetAsync()
    {
        Profiles = await _profileService.GetAllAsync();
    }

    public async Task<IActionResult> OnPostSetActiveAsync(int id)
    {
        await _profileService.SetActiveAsync(id);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        await _profileService.DeleteAsync(id);
        return RedirectToPage();
    }
}
