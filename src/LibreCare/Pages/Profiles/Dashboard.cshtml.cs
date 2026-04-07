using LibreCare.Data;
using LibreCare.Models;
using LibreCare.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LibreCare.Pages.Profiles;

public class DashboardModel : PageModel
{
    private readonly ProfileService _profileService;

    public DashboardModel(ProfileService profileService)
    {
        _profileService = profileService;
    }

    public Profile? Profile { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Profile = await _profileService.GetByIdAsync(id);

        if (Profile == null) return NotFound();

        return Page();
    }
}
