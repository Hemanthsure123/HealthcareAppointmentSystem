using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Azure.Communication.Identity;
using System.Threading.Tasks;
using System.IO;
using System;
using System.Collections.Generic;
using System.Text;
using AppointmentService.Services; // For BlobStorageService

namespace AppointmentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TelemedicineController : ControllerBase
    {
        private readonly CommunicationIdentityClient _acsClient;
        private readonly BlobStorageService _blobStorageService;
        // This dictionary will store a call ID for each appointment ID
        private static readonly Dictionary<string, Guid> _appointmentCallIds = new();

        public TelemedicineController(CommunicationIdentityClient acsClient, BlobStorageService blobStorageService)
        {
            _acsClient = acsClient;
            _blobStorageService = blobStorageService;
        }

        [HttpPost("token/{appointmentId}")]
        public async Task<IActionResult> GetAcsToken(string appointmentId)
        {
            // Get or create a unique group ID for the appointment call
            if (!_appointmentCallIds.ContainsKey(appointmentId))
            {
                _appointmentCallIds[appointmentId] = Guid.NewGuid();
            }
            var groupId = _appointmentCallIds[appointmentId];

            var identityResponse = await _acsClient.CreateUserAsync();
            var identity = identityResponse.Value;

            var tokenResponse = await _acsClient.GetTokenAsync(identity, scopes: new[] { new CommunicationTokenScope("voip") });
            var token = tokenResponse.Value;

            return Ok(new
            {
                acsUserId = identity.Id,
                token = token.Token,
                expiresOn = token.ExpiresOn,
                groupId = groupId // Return the group ID for the call
            });
        }

        [HttpPost("recording")]
        public async Task<IActionResult> UploadRecording()
        {
            // This is a simulation of uploading a recording file
            string mockRecordingContent = "This is the content of the call recording.";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(mockRecordingContent));
            string fileName = $"recording-{System.Guid.NewGuid()}.txt";

            await _blobStorageService.UploadAsync("telemedicine-recordings", fileName, stream);

            return Ok(new { message = "Recording uploaded successfully", file = fileName });
        }
    }
}