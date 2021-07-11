using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace PortalAsp.Core.Helpers.IdentityExtensions
{
    public static class IdentityExtensions
    {
        public static string GetUserIdFromClaims(this HttpContext context)
        {
            return context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
