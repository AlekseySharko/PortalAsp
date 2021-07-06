using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PortalAsp.Filters;

namespace PortalAsp.Controllers.Auth
{
    [Route("api/auth/test")]
    public class AuthTest : Controller
    {
        private RoleManager<IdentityRole> RoleManager { get; }
        public AuthTest(RoleManager<IdentityRole> roleManager) => RoleManager = roleManager;

        [HttpGet]
        [Route("authorized")]
        public IActionResult GetAuthorized()
        {
            return Ok("Test get");
        }

        [HttpPost]
        [Authorize]
        [Route("authorized")]
        public async Task<IActionResult> PostAuthorized()
        {
            IdentityRole role = new IdentityRole { Name = "Catalog Moderator" };
            IdentityResult result = await RoleManager.CreateAsync(role);
            return Ok("You are authorized");
        }

        [HttpPost]
        [RoleAuthFilter("Catalog Moderator")]
        [Route("role")]
        public IActionResult PostRoles()
        {
            return Ok("You are CatalogModerator");
        }
    }
}
