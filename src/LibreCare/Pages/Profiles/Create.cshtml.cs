using LibreCare.Data;
using LibreCare.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LibreCare.Pages.Profiles;

public class CreateModel : PageModel
{
    private readonly LibreCareContext _context;

    public CreateModel(LibreCareContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Profile Profile { get; set; } = new();

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        // Deactivate all existing profiles
        var profiles = await _context.Profiles.ToListAsync();
        foreach (var p in profiles)
        {
            p.IsActive = false;
        }

        // Make new profile active
        Profile.IsActive = true;

        _context.Profiles.Add(Profile);
        await _context.SaveChangesAsync();

        return RedirectToPage("Index");
    }
}
