using LibreCare.Data;
using LibreCare.Models;
using LibreCare.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LibreCare.Pages.Profiles;

public class EditModel : PageModel
{
    private readonly ProfileService _profileService;

    public EditModel(ProfileService profileService)
    {
        _profileService = profileService;
    }

    [BindProperty] public Profile Profile { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null) return NotFound();

        var profile = await _profileService.GetByIdAsync(id.Value);

        if (profile == null) return NotFound();

        Profile = profile;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        await _profileService.UpdateAsync(Profile);

        return RedirectToPage("Index");
    }
}
