using LibreCare.Data;
using LibreCare.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LibreCare.Pages.Profiles;

public class DashboardModel : PageModel
{
    private readonly LibreCareContext _context;

    public DashboardModel(LibreCareContext context)
    {
        _context = context;
    }

    public Profile? Profile { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Profile = await _context.Profiles.FindAsync(id);

        if (Profile == null)
        {
            return NotFound();
        }

        return Page();
    }
}
