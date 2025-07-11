using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClientApp.Services;
using System.Threading.Tasks;

namespace ClientApp.Controllers
{
    [Authorize(Roles = "Admin")] // This secures the entire controller for Admins only
    public class AdminController : Controller
    {
        private readonly ApiService _apiService;

        public AdminController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var pendingUsers = await _apiService.GetPendingUsersAsync();
            return View(pendingUsers);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveUser(string userId)
        {
            await _apiService.ApproveUserAsync(userId);
            return RedirectToAction(nameof(Index));
        }
    }
}