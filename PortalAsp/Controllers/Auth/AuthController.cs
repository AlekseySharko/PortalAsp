using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalAsp.Core.Helpers.IdentityExtensions;
using PortalModels;
using PortalModels.Authentication;
using PortalModels.Validators;
using PortalModels.Validators.Authentication;

namespace PortalAsp.Controllers.Auth
{
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private IUserAuthenticator AuthenticationService { get; }
        public AuthController(IUserAuthenticator authenticationService)
        {
            AuthenticationService = authenticationService;
        }
        
        [HttpPost]
        [Route("sign-up")]
        public async Task<IActionResult> SignUp([FromBody] AuthenticationUserData user)
        {
            ValidationResult validationResult = AuthenticationValidator.ValidateOnSignUp(user);
            if (!validationResult.IsValid) return BadRequest(validationResult.Message);

            GeneralResult result = await AuthenticationService.SignUpAsync(user);
            if (result.Success) return Ok();

            return BadRequest(result.Message);
        }

        [HttpPost]
        [Route("log-in")]
        public async Task<IActionResult> LogIn([FromBody] AuthenticationUserData user)
        {
            ValidationResult validationResult = AuthenticationValidator.ValidateOnLogIn(user);
            if (!validationResult.IsValid) return BadRequest(validationResult.Message);

            LoginSuccessfulData loginData = await AuthenticationService.LogInOrReturnNullAsync(user);
            if (loginData == null) return BadRequest("Wrong username or password");
            return Ok(loginData);
        }

        [HttpGet]
        [Authorize]
        [Route("role")]
        public async Task<IActionResult> CheckRole([FromQuery] string role)
        {
            string id = HttpContext.GetUserIdFromClaims();
            return Ok(await AuthenticationService.CheckRoleAsync(id, role));
        }
    }
}
