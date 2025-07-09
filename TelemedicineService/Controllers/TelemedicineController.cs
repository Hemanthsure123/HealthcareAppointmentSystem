using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Azure.Communication.Identity;
using System.Threading.Tasks;
using AppointmentService.Services;
using System.IO;
using System.Text;

namespace AppointmentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TelemedicineController : ControllerBase
    {
        private readonly CommunicationIdentityClient _acsClient;
        private readonly BlobStorageService _blobStorageService;

        // The dependencies are now injected here
        public TelemedicineController(CommunicationIdentityClient acsClient, BlobStorageService blobStorageService)
        {
            _acsClient = acsClient;
            _blobStorageService = blobStorageService;
        }

        [HttpPost("token")]
        public async Task<IActionResult> GetAcsToken()
        {
            var identityResponse = await _acsClient.CreateUserAsync();
            var identity = identityResponse.Value;

            var tokenResponse = await _acsClient.GetTokenAsync(identity, scopes: new[] { new CommunicationTokenScope("voip") });
            var token = tokenResponse.Value;

            return Ok(new
            {
                acsUserId = identity.Id,
                token = token.Token,
                expiresOn = token.ExpiresOn
            });
        }

        [HttpPost("recording")]
        public async Task<IActionResult> UploadRecording()
        {
            string mockRecordingContent = "This is the content of the call recording.";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(mockRecordingContent));
            string fileName = $"recording-{System.Guid.NewGuid()}.txt";

            await _blobStorageService.UploadAsync("telemedicine-recordings", fileName, stream);

            return Ok(new { message = "Recording uploaded successfully", file = fileName });
        }
    }
}