using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LibreCare.Services; // your ProfileService namespace

public class IndexModel : PageModel
{
    private readonly ProfileService _profileService;

    public IndexModel(ProfileService profileService)
    {
        _profileService = profileService;
    }

    public IActionResult OnGet()
    {
        var profiles = _profileService.GetAllProfiles();

        if (!profiles.Any())
        {
            // First time launch, no profiles exist
            return RedirectToPage("/Profiles/Create");
        }

        var defaultProfile = _profileService.GetDefaultProfile();
        if (defaultProfile != null)
        {
            // Redirect to default profile dashboard
            return RedirectToPage("/Profiles/Dashboard", new { id = defaultProfile.Id });
        }

        // Profiles exist but no default → show selection page
        return RedirectToPage("/Profiles/Select");
    }
}
