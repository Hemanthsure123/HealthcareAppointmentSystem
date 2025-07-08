using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UserService.Models;

[Route("api/[controller]")]
[ApiController]
public class RolesController : ControllerBase
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<User> _userManager;

    public RolesController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    [HttpPost("createrole")]
    public async Task<IActionResult> CreateRole(string roleName)
    {
        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
            if (result.Succeeded)
            {
                return Ok(new { Message = $"Role '{roleName}' created successfully!" });
            }
            return BadRequest(result.Errors);
        }
        return Conflict(new { Message = "Role already exists." });
    }

    [HttpPost("assignrole")]
    public async Task<IActionResult> AssignRole(string userEmail, string roleName)
    {
        var user = await _userManager.FindByEmailAsync(userEmail);
        if (user == null)
        {
            return NotFound(new { Message = "User not found." });
        }

        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            return BadRequest(new { Message = "Role does not exist." });
        }

        var result = await _userManager.AddToRoleAsync(user, roleName);
        if (result.Succeeded)
        {
            return Ok(new { Message = $"Role '{roleName}' assigned to user '{userEmail}' successfully!" });
        }
        return BadRequest(result.Errors);
    }
}