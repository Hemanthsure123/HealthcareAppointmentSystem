using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UserService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    [HttpGet("test")]
    [Authorize(Roles = "Admin")] // This protects the endpoint
    public IActionResult GetTest()
    {
        return Ok("The UserService is running and the Admin role is working!");
    }
}