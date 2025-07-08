using Castle.Core.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserService.Controllers;
using UserService.Models;
using Xunit;

namespace UserService.Tests
{
    public class AuthControllerTests
    {
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly Mock<Microsoft.Extensions.Configuration.IConfiguration> _mockConfiguration;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            // We need to mock the UserStore, which UserManager depends on
            var store = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

            _mockConfiguration = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();

            // Set up mock configuration for JWT settings if needed for login tests later
            var jwtSection = new Mock<IConfigurationSection>();
            jwtSection.Setup(s => s["Key"]).Returns("ThisIsA_SuperSecretKey_For_My_Healthcare_App_12345!");
            jwtSection.Setup(s => s["Issuer"]).Returns("http://localhost");
            jwtSection.Setup(s => s["Audience"]).Returns("http://localhost");
            _mockConfiguration.Setup(c => c.GetSection("JWT")).Returns(jwtSection.Object);

            _controller = new AuthController(_mockUserManager.Object, _mockConfiguration.Object);
        }

        [Fact]
        public async Task Register_WithNewEmail_ReturnsOk()
        {
            // Arrange: Set up the scenario
            var registerModel = new RegisterModel { Email = "newuser@example.com", Password = "Password123!" };

            // Tell the mock UserManager that no user exists with this email
            _mockUserManager.Setup(x => x.FindByEmailAsync(registerModel.Email)).ReturnsAsync((User)null);

            // Tell the mock UserManager to return a successful result when CreateAsync is called
            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), registerModel.Password)).ReturnsAsync(IdentityResult.Success);

            // Act: Execute the method being tested
            var result = await _controller.Register(registerModel);

            // Assert: Verify the outcome
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Register_WithExistingEmail_ReturnsConflict()
        {
            // Arrange
            var registerModel = new RegisterModel { Email = "existinguser@example.com", Password = "Password123!" };

            // Tell the mock UserManager that a user *does* exist with this email
            _mockUserManager.Setup(x => x.FindByEmailAsync(registerModel.Email)).ReturnsAsync(new User());

            // Act
            var result = await _controller.Register(registerModel);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(409, statusCodeResult.StatusCode); // 409 Conflict
        }
    }
}