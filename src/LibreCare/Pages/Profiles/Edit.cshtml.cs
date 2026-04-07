using LibreCare.Data;
using LibreCare.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LibreCare.Pages.Profiles;

public class EditModel : PageModel
{
    private readonly LibreCareContext _context;

    public EditModel(LibreCareContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Profile Profile { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var profile = await _context.Profiles.FindAsync(id);

        if (profile == null)
        {
            return NotFound();
        }

        Profile = profile;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var existingProfile = await _context.Profiles.FindAsync(Profile.Id);

        if (existingProfile == null)
        {
            return NotFound();
        }

        // Update fields (controlled update)
        existingProfile.Name = Profile.Name;
        existingProfile.BirthDate = Profile.BirthDate;

        await _context.SaveChangesAsync();

        return RedirectToPage("Index");
    }
}
