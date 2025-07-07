using Microsoft.AspNetCore.Mvc;

namespace UserService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    [HttpGet("test")]
    public IActionResult GetTest()
    {
        return Ok("The UserService is running and reachable through the gateway!");
    }
}