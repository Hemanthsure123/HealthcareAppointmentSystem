using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Xunit;
using AppointmentService.Controllers;
using AppointmentService.Services;
using Azure.Communication.Identity;
using Azure;
using Azure.Communication;
using System;
using Microsoft.Extensions.Configuration;
using Azure.Core;

namespace AppointmentService.Tests
{
    public class TelemedicineControllerTests
    {
        private readonly Mock<CommunicationIdentityClient> _mockAcsClient;
        private readonly Mock<BlobStorageService> _mockBlobStorageService;
        private readonly TelemedicineController _controller;

        public TelemedicineControllerTests()
        {
            _mockAcsClient = new Mock<CommunicationIdentityClient>();

            var mockConfiguration = new Mock<IConfiguration>();
            _mockBlobStorageService = new Mock<BlobStorageService>(mockConfiguration.Object);

            _controller = new TelemedicineController(_mockAcsClient.Object, _mockBlobStorageService.Object);
        }

        [Fact]
        public async Task GetAcsToken_WhenCalled_ReturnsOkWithToken()
        {
            // Arrange
            var fakeIdentity = CommunicationIdentity.FromRawId("fake-user-id");
            var fakeToken = new AccessToken("fake-access-token", DateTimeOffset.Now.AddHours(1));

            _mockAcsClient.Setup(x => x.CreateUserAsync(It.IsAny<System.Threading.CancellationToken>()))
                .ReturnsAsync(Response.FromValue(fakeIdentity, new Mock<Response>().Object));

            _mockAcsClient.Setup(x => x.GetTokenAsync(It.IsAny<CommunicationUserIdentifier>(), It.IsAny<System.Collections.Generic.IEnumerable<CommunicationTokenScope>>(), It.IsAny<System.Threading.CancellationToken>()))
                .ReturnsAsync(Response.FromValue(fakeToken, new Mock<Response>().Object));

            // Act
            var result = await _controller.GetAcsToken();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }
    }
}