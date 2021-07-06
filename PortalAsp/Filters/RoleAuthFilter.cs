using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using PortalModels.Authentication;

namespace PortalAsp.Filters
{
    public class RoleAuthFilter : Attribute, IAsyncAuthorizationFilter, IOrderedFilter
    {
        public int Order { get; set; } = 4;
        private string RequiredRole { get; }

        public RoleAuthFilter(string requiredRole) => RequiredRole = requiredRole;

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
                return;
            }
            
            IUserAuthenticator authService = context.HttpContext.RequestServices.GetService<IUserAuthenticator>();
            string userId =
                context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userid")?.Value;
            bool result = await authService.CheckRole(userId, RequiredRole);

            if (!result)
            {
                context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
            }
        }
    }
}
