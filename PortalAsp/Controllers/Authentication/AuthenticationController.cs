using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PortalAsp.Validators;
using PortalAsp.Validators.Authentication;
using PortalModels;
using PortalModels.Authentication;

namespace PortalAsp.Controllers.Authentication
{
    [Route("api/catalog/authentication")]
    public class AuthenticationController : Controller
    {
        private IUserAuthenticator AuthenticationService { get; }
        public AuthenticationController(IUserAuthenticator authenticationService)
        {
            AuthenticationService = authenticationService;
        }
        
        [HttpPost]
        [Route("sign-up")]
        public async Task<IActionResult> SignUp([FromBody] AuthenticationUserData user)
        {
            ValidationResult validationResult = AuthenticationValidator.ValidateOnSignUp(user);
            if (!validationResult.IsValid) return BadRequest(validationResult.Message);

            GeneralResult result = await AuthenticationService.SignUp(user);
            if (result.Success) return Ok();

            return BadRequest(result.Message);
        }

        [HttpPost]
        [Route("log-in")]
        public async Task<IActionResult> LogIn([FromBody] AuthenticationUserData user)
        {
            ValidationResult validationResult = AuthenticationValidator.ValidateOnLogIn(user);
            if (!validationResult.IsValid) return BadRequest(validationResult.Message);

            await AuthenticationService.LogIn(user);

            return Ok();
        }
    }
}
