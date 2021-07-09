using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using PortalModels.Authentication;

namespace PortalAsp.Filters
{
    public class RoleAuthFilter : Attribute, IAsyncAuthorizationFilter
    {
        private string RequiredRole { get; }

        public RoleAuthFilter(string requiredRole) => RequiredRole = requiredRole;

        //TODO - test
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if(CheckForOtherAuthAttributes(context)) return;
            
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
                return;
            }
            
            IUserAuthenticator authService = context.HttpContext.RequestServices.GetService<IUserAuthenticator>();
            string userId =
                context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            bool result = await authService.CheckRole(userId, RequiredRole);

            if (!result)
            {
                context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
            }
        }

        private bool CheckForOtherAuthAttributes(AuthorizationFilterContext context)
        {
            if (MetadataContainsCheck(context, "Microsoft.AspNetCore.Authorization.AuthorizeAttribute"))
            {
                return true;
            }
            if (MetadataContainsCheck(context, "Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute"))
            {
                return true;
            }

            return false;
        }

        private bool MetadataContainsCheck(AuthorizationFilterContext context, string searchFor)
        {
            if (context.ActionDescriptor.EndpointMetadata.FirstOrDefault(m => m != null && m.ToString() == searchFor) != null)
            {
                return true;
            }
            return false;
        }
    }
}
