using Microsoft.AspNetCore.Mvc;
using ClientApp.Services;
using System.Threading.Tasks;
using Shared.Models;
using ClientApp.Models; // Add this using statement
using System.Text.Json; // Add this using statement

namespace ClientApp.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly ApiService _apiService;

        public AppointmentsController(ApiService apiService)
        {
            _apiService = apiService;
        }

        // GET: /Appointments
        public async Task<IActionResult> Index()
        {
            var appointments = await _apiService.GetAppointmentsAsync();
            return View(appointments);
        }

        // GET: /Appointments/Create
        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> JoinCall(Guid id)
        {
            var acsTokenInfoJson = await _apiService.GetAcsTokenAsync(id.ToString());
            var acsTokenModel = JsonSerializer.Deserialize<AcsTokenModel>(acsTokenInfoJson.ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View("Call", acsTokenModel);
        }

        // POST: /Appointments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PatientId,DoctorId,Date,IsVirtual")] AppointmentDTO appointment)
        {
            if (ModelState.IsValid)
            {
                await _apiService.CreateAppointmentAsync(appointment);
                return RedirectToAction(nameof(Index));
            }
            return View(appointment);
        }
    }
}