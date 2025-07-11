using Microsoft.AspNetCore.Identity;

namespace UserService.Models;

public class User : IdentityUser
{
    // New properties for the approval workflow and doctor details
    public bool IsApproved { get; set; } = false;
    public string? Hospital { get; set; } // We'll use simple strings for now
    public string? Specialty { get; set; }
}